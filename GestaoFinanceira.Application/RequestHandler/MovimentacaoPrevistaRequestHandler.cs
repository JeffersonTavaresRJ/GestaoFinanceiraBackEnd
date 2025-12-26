using AutoMapper;
using FluentValidation;
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
using GestaoFinanceira.Domain.Exceptions.Movimentacao;
using FluentValidation.Results;

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

            List<MovimentacaoPrevista> movimentacoesPrevistas = new List<MovimentacaoPrevista>();
            IList<ValidationFailure> errors = new List<ValidationFailure>();

            foreach (MovimentacaoPrevistaCommand item in request.MovimentacaoPrevistaCommand)
            {
                if (item.TipoPrioridade == null)
                {

                    errors.Add(new ValidationFailure("TipoPrioridade", "A Prioridade é obrigatória"));
                    throw new ValidationException(errors);
                }

                if (item.DataVencimento.Year != item.DataReferencia.Year ||
                    item.DataVencimento.Month != item.DataReferencia.Month)
                {
                    throw new MovDataReferenciaException("Data de Vencimento", item.DataVencimento, item.DataReferencia);
                }

                MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(item);

                var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
                if (!validate.IsValid)
                {
                    throw new ValidationException(validate.Errors);
                }

                movimentacoesPrevistas.Add(movimentacaoPrevista);

            }

            movimentacoesPrevistas = movimentacaoPrevistaDomainService.AddResult(movimentacoesPrevistas);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = movimentacoesPrevistas,
                Action = ActionNotification.Criar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(UpdateMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {

            if (request.DataVencimento.Year != request.DataReferencia.Year ||
               request.DataVencimento.Month != request.DataReferencia.Month)
            {
                throw new MovDataReferenciaException("Data de Vencimento", request.DataVencimento, request.DataReferencia);
            }

            if (request.TipoPrioridade == null)
            {
                IList<ValidationFailure> errors = new List<ValidationFailure>();
                errors.Add(new ValidationFailure("TipoPrioridade", "A Prioridade é obrigatória"));
                throw new ValidationException(errors);
            }

            MovimentacaoPrevista movimentacaoPrevista = mapper.Map<MovimentacaoPrevista>(request);

            var validate = new MovimentacaoPrevistaValidation().Validate(movimentacaoPrevista);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }

            Movimentacao movimentacao = movimentacaoDomainService.GetByKey(movimentacaoPrevista.IdItemMovimentacao,
                                                                           movimentacaoPrevista.DataReferencia);

            var sumValorReal = 0d;

            foreach (var item in movimentacao.MovimentacoesRealizadas)
            {
                sumValorReal += item.Valor;
            }

            if (sumValorReal == movimentacaoPrevista.Valor && !movimentacaoPrevista.Status.Equals(StatusMovimentacaoPrevista.Q))
            {
                throw new MovPrevStatusInvalidoException(movimentacao.ItemMovimentacao.Descricao,
                                                              movimentacao.DataReferencia,
                                                              StatusMovimentacaoPrevista.Q);
            }

            if (sumValorReal != movimentacaoPrevista.Valor && movimentacaoPrevista.Status.Equals(StatusMovimentacaoPrevista.Q))
            {
                throw new MovPrevStatusInvalidoException(movimentacao.ItemMovimentacao.Descricao,
                                                              movimentacao.DataReferencia,
                                                              StatusMovimentacaoPrevista.A);
            }
            /*
            if (movimentacaoPrevista.Status.Equals(StatusMovimentacaoPrevista.N))
            {
                throw new MovPrevStatusInvalidoException(movimentacao.ItemMovimentacao.Descricao,
                                                         movimentacao.DataReferencia,
                                                         movimentacaoPrevista.Status);
            }
            */

            movimentacaoPrevista = movimentacaoPrevistaDomainService.UpdateResult(movimentacaoPrevista);

            var listaUpdate = new List<MovimentacaoPrevista>();
            listaUpdate.Add(movimentacaoPrevista);

            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = listaUpdate,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoPrevistaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoPrevista movimentacaoPrevista = movimentacaoPrevistaDomainService.GetId(request.Id);
            var listaDelete = new List<MovimentacaoPrevista>();
            listaDelete.Add(movimentacaoPrevista);
            ;
            if (!(movimentacaoPrevista.NrParcela == 1 || movimentacaoPrevista.NrParcela == movimentacaoPrevista.NrParcelaTotal))
            {
                throw new MovPrevParcelaInvalidaExclusaoException(movimentacaoPrevista.NrParcela, movimentacaoPrevista.NrParcelaTotal);
            }

            var listaUpdate = new List<MovimentacaoPrevista>();
            movimentacaoPrevistaDomainService.Delete(movimentacaoPrevista, out listaUpdate);

            //exclui a previsão no mongo db..
            _ = mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = listaDelete,
                Action = ActionNotification.Excluir
            });

            //atualiza as movimentações previstas parceladas..
            await mediator.Publish(new MovimentacaoPrevistaNotification
            {
                MovimentacoesPrevistas = listaUpdate,
                Action = ActionNotification.Atualizar
            });

            return Unit.Value;
        }

        /*
        private List<MovimentacaoPrevista> ConvertList(MovimentacaoPrevista obj, string tipoRecorrencia, int qtdeParcelas)
        {
            if(tipoRecorrencia.Equals(TipoRecorrencia.M.ToString()))
            {
                qtdeParcelas = (13 - obj.DataReferencia.Month)==0? 13 : 13 - obj.DataReferencia.Month;
            }
            else if (tipoRecorrencia.Equals(TipoRecorrencia.N.ToString()))
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
                    NrParcela = tipoRecorrencia.Equals(TipoRecorrencia.M.ToString()) ? 1 : i,
                    NrParcelaTotal = tipoRecorrencia.Equals(TipoRecorrencia.M.ToString()) ? 1 : qtdeParcelas
                };
                lista.Add(mov);
            }
            return lista;

        }
        */
    }
}