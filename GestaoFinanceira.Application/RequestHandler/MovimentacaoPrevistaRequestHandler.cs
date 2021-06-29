using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.Enuns;
using GestaoFinanceira.Application.Commands.MovimentacaoPrevista;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GestaoFinanceira.Domain.Models.Enuns;

namespace GestaoFinanceira.Application.RequestHandler
{
    public class MovimentacaoPrevistaRequestHandler : IRequestHandler<CreateMovimentacaoPrevistaCommand>,
                                                      IRequestHandler<UpdateMovimentacaoPrevistaCommand>,
                                                      IRequestHandler<DeleteMovimentacaoPrevistaCommand>
    {
        private readonly IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService;
        private readonly IMovimentacaoDomainService movimentacaoDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public MovimentacaoPrevistaRequestHandler(IMovimentacaoPrevistaDomainService movimentacaoPrevistaDomainService,
                                                  IMovimentacaoDomainService movimentacaoDomainService,
                                                  IMediator mediator, 
                                                  IMapper mapper)
        {
            this.movimentacaoPrevistaDomainService = movimentacaoPrevistaDomainService;
            this.movimentacaoDomainService = movimentacaoDomainService;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            if(request.QtdeParcelas <=0)
            {
                throw new TotalParcelasMovimentacaoInvalidoException(0);
            }

            if(request.TipoRecorrencia != TipoRecorrenciaMovimentacaoPrevista.M.ToString() &&
               request.TipoRecorrencia != TipoRecorrenciaMovimentacaoPrevista.N.ToString() &&
               request.TipoRecorrencia != TipoRecorrenciaMovimentacaoPrevista.P.ToString())
            {
                throw new TipoRecorrenciaMovimentacaoInvalidoException();
            }
            
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);

            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            List<MovimentacaoPrevista> movimentacoesPrevistas = ConvertList(movimentacaoPrevista, request.TipoRecorrencia, request.QtdeParcelas);
            movimentacaoPrevistaDomainService.Add(movimentacoesPrevistas);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = movimentacoesPrevistas,
                Action = ActionNotification.Criar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            
            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);            
            
            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }

            Movimentacao movimentacao = movimentacaoDomainService.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                           movimentacaoPrevista.DataReferencia);

            if (movimentacao.MovimentacoesRealizadas.Count == 0 && movimentacaoPrevista.Status.Equals(StatusMovimentacaoPrevista.Q) ||
                movimentacao.MovimentacoesRealizadas.Count > 0 && !movimentacaoPrevista.Status.Equals(StatusMovimentacaoPrevista.Q))
            {
                throw new StatusMovimentacaoInvalidoException(movimentacao.ItemMovimentacao.Descricao, 
                                                              movimentacao.DataReferencia, 
                                                              StatusMovimentacaoPrevista.Q);
            }

            movimentacaoPrevistaDomainService.Update(movimentacaoPrevista);
            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = movimentacaoPrevistaDomainService.GetByKey(request.IdItemMovimentacao, request.DataReferencia);
;           movimentacaoPrevistaDomainService.Delete(movimentacaoPrevista);           
            
            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacaoPrevista = movimentacaoPrevista,
                Action = ActionNotification.Excluir
            });
            

            return Unit.Value;
        }

        private List<MovimentacaoPrevista> ConvertList(MovimentacaoPrevista obj, string tipoRecorrencia, int qtdeParcelas)
        {
            if(tipoRecorrencia.Equals(TipoRecorrenciaMovimentacaoPrevista.M.ToString()))
            {
                qtdeParcelas = (13 - obj.DataReferencia.Month)==0? 13 : 13 - obj.DataReferencia.Month;
            }
            else if (tipoRecorrencia.Equals(TipoRecorrenciaMovimentacaoPrevista.N.ToString()))
            {
                qtdeParcelas = 1;
            }
            
            List<MovimentacaoPrevista> lista = new List<MovimentacaoPrevista>();

            for (int i = 1; i <= qtdeParcelas; i++)
            {
                MovimentacaoPrevista mov = new MovimentacaoPrevista
                {
                    Movimentacao = new Movimentacao
                    {
                        DataReferencia = obj.Movimentacao.DataReferencia.AddMonths(i - 1),
                        IdItemMovimentacao = obj.Movimentacao.IdItemMovimentacao,
                        ItemMovimentacao = obj.Movimentacao.ItemMovimentacao,
                        MovimentacaoPrevista = obj.Movimentacao.MovimentacaoPrevista,
                        MovimentacoesRealizadas = obj.Movimentacao.MovimentacoesRealizadas,
                        Observacao = obj.Movimentacao.Observacao,
                        TipoPrioridade = obj.Movimentacao.TipoPrioridade
                    },
                    DataReferencia = obj.DataReferencia.AddMonths(i - 1),
                    DataVencimento = obj.DataVencimento.AddMonths(i - 1),
                    FormaPagamento = obj.FormaPagamento,
                    IdFormaPagamento = obj.IdFormaPagamento,
                    IdItemMovimentacao = obj.IdItemMovimentacao,
                    Status = obj.Status,
                    Valor = obj.Valor,
                    NrParcela = tipoRecorrencia.Equals(TipoRecorrenciaMovimentacaoPrevista.M.ToString()) ? 1 : i,
                    NrParcelaTotal = tipoRecorrencia.Equals(TipoRecorrenciaMovimentacaoPrevista.M.ToString()) ? 1 : qtdeParcelas
                };
                lista.Add(mov);
            }
            return lista;

        }
    }
}