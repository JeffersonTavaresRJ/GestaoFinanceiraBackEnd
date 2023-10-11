using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Commands.TransferenciaConta;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoRealizada;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{


    public class MovimentacaoRealizadaRequestHandler : IRequestHandler<CreateMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<UpdateMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<DeleteMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<TransferenciaContaCommand>
    {
        private readonly IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService;
        private readonly ISaldoDiarioDomainService saldoDiarioDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private MovimentacaoPrevista movimentacaoPrevista;
        private List<SaldoDiario> saldosDiario = new List<SaldoDiario>();
        private List<MovimentacaoRealizada> movimentacoesRealizadas = new List<MovimentacaoRealizada>();
        private List<MovimentacaoPrevista> movimentacoesPrevistas = new List<MovimentacaoPrevista>();


        public MovimentacaoRealizadaRequestHandler(IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService,
                                                   ISaldoDiarioDomainService saldoDiarioDomainService,
                                                   IMediator mediator, 
                                                   IMapper mapper)
        {
            this.movimentacaoRealizadaDomainService = movimentacaoRealizadaDomainService;
            this.saldoDiarioDomainService = saldoDiarioDomainService;
            this.mediator = mediator;
            this.mapper = mapper; 
        }

        public async Task<Unit> Handle(CreateMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            /*==Gravação da Movimentação Realizada==*/

            MovimentacaoRealizada movimentacaoRealizada = mapper.Map<MovimentacaoRealizada>(request);

            var validate = new MovimentacaoRealizadaValidation().Validate(movimentacaoRealizada);
            if (!validate.IsValid)
            {
                 throw new ValidationException(validate.Errors);
            }

            
            /*adicionando no banco de dados..*/
            movimentacaoRealizada = movimentacaoRealizadaDomainService.Add(movimentacaoRealizada, out movimentacaoPrevista);
            movimentacoesRealizadas.Add(movimentacaoRealizada);

            /*adicionando no mongoDB..*/
            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Criar
            });


            /*==Atualização do Saldo Diário no MongoDB==*/

            /*Lista todos os Saldos Diários da conta corrente, com data >= à data de movimentação */
            foreach (var item in saldoDiarioDomainService.GetBySaldosDiario(movimentacaoRealizada.IdConta, movimentacaoRealizada.DataMovimentacaoRealizada))
            {
                saldosDiario.Add(item);
            }


            /*adicionando no mongoDB..*/
            await mediator.Publish(new SaldoDiarioNotification
            {
                SaldosDiario = saldosDiario,
                Action = ActionNotification.Atualizar
            });



            /*==Atualização do Status das Movimentações Previstas no MongoDB==*/
            if (movimentacaoPrevista != null)
            {
                movimentacoesPrevistas.Add(movimentacaoPrevista);
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacoesPrevistas = movimentacoesPrevistas,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                              movimentacaoPrevista.DataReferencia,
                                              movimentacaoPrevista.Status/*,
                                              movimentacaoRealizada.Id*/);
            }
            else
            {
                throw new MovRealSucessoException(/*movimentacaoRealizada.Id*/);
            }
        }

        public async Task<Unit> Handle(UpdateMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoRealizada movimentacaoRealizada    = mapper.Map<MovimentacaoRealizada>(request);
            MovimentacaoRealizada movimentacaoRealizadaOld = movimentacaoRealizadaDomainService.GetId(movimentacaoRealizada.Id);
            

            var validate = new MovimentacaoRealizadaValidation().Validate(movimentacaoRealizada);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            
            movimentacaoRealizadaDomainService.Update(movimentacaoRealizada, out movimentacaoPrevista);
            movimentacoesRealizadas.Add(movimentacaoRealizada);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Atualizar
            });

            /*==Atualização do Saldo Diário no MongoDB==*/

            /*Verifica se o Saldo Diário antigo existe para a conta e data de movimentação realizada..*/
            var saldo = saldoDiarioDomainService.GetByKey(movimentacaoRealizadaOld.IdConta, movimentacaoRealizadaOld.DataMovimentacaoRealizada);
            if (saldo == null)
            {
                /*Se não existir, trata para excluir no MongoDB..*/
                saldosDiario.Add(new SaldoDiario { IdConta = movimentacaoRealizadaOld.IdConta, DataSaldo = movimentacaoRealizadaOld.DataMovimentacaoRealizada });
            }

            /*Lista todos os Saldos Diários da conta antiga, com data >= à data de movimentação */
            if (movimentacaoRealizadaOld.IdConta != movimentacaoRealizada.IdConta)
            {
                foreach (var item in saldoDiarioDomainService.GetBySaldosDiario(movimentacaoRealizadaOld.IdConta, movimentacaoRealizadaOld.DataMovimentacaoRealizada))
                {
                    saldosDiario.Add(item);
                }
            }

            /*Tratamento para identificar a menor data de movimentação, caso venha ser alterada..*/
            DateTime dataMovimentacaoRealizada = movimentacaoRealizadaOld.DataMovimentacaoRealizada < movimentacaoRealizada.DataMovimentacaoRealizada ?
                                                 movimentacaoRealizadaOld.DataMovimentacaoRealizada : movimentacaoRealizada.DataMovimentacaoRealizada;

            /*Lista todos os Saldos Diários da conta corrente, com data >= à data de movimentação */
            foreach (var item in saldoDiarioDomainService.GetBySaldosDiario(movimentacaoRealizada.IdConta, dataMovimentacaoRealizada))
            {
                saldosDiario.Add(item);
            }


            /*adicionando no mongoDB..*/
            await mediator.Publish(new SaldoDiarioNotification
             {
                 SaldosDiario = saldosDiario,
                 Action = ActionNotification.Atualizar
             });

            /*==Atualização do Status das Movimentações Previstas no MongoDB==*/
            if (movimentacaoPrevista != null)
            {
                movimentacoesPrevistas.Add(movimentacaoPrevista);
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacoesPrevistas = movimentacoesPrevistas,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status/*, null*/);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoRealizada movimentacaoRealizada = movimentacaoRealizadaDomainService.GetId(request.Id);
            movimentacaoRealizadaDomainService.Delete(movimentacaoRealizada, out movimentacaoPrevista);

            movimentacoesRealizadas.Add(movimentacaoRealizada);
            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Excluir
            });

            /*==Atualização do Saldo Diário no MongoDB==*/

            /*Verifica se o Saldo Diário da Movimentação Realizada excluída existe para a conta e data de movimentação realizada..*/
            var saldo = saldoDiarioDomainService.GetByKey(movimentacaoRealizada.IdConta, movimentacaoRealizada.DataMovimentacaoRealizada);

            if (saldo == null)
            {
                /*Se não existir, trata para excluir..*/
                saldosDiario.Add(new SaldoDiario { IdConta = movimentacaoRealizada.IdConta, DataSaldo = movimentacaoRealizada.DataMovimentacaoRealizada });
            }

            /*lista todos os Saldos Diários correntes a partir da conta, com data >= à data de movimentação */
            foreach (var item in saldoDiarioDomainService.GetBySaldosDiario(movimentacaoRealizada.IdConta, movimentacaoRealizada.DataMovimentacaoRealizada))
            {
                saldosDiario.Add(item);
            }

            await mediator.Publish(new SaldoDiarioNotification
             {
                SaldosDiario = saldosDiario,
                Action = ActionNotification.Excluir
             });



            /*==Atualização do Status das Movimentações Previstas no MongoDB==*/
            if (movimentacaoPrevista != null)
            {
                movimentacoesPrevistas.Add(movimentacaoPrevista);
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacoesPrevistas = movimentacoesPrevistas,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status/*, null*/);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(TransferenciaContaCommand request, CancellationToken cancellationToken)
        {
            TransferenciaContas transferenciaConta = mapper.Map<TransferenciaContas>(request);

            var validate = new TransferenciaContaValidation().Validate(transferenciaConta);
            if (!validate.IsValid)
            {
                throw new ValidationException(validate.Errors);
            }
            
            /*adicionando no banco de dados..*/
            movimentacoesRealizadas = movimentacaoRealizadaDomainService.ExecutarTransferencia(transferenciaConta);


            /*adicionando no mongoDB..*/
            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Criar
            });


            /*==Atualização do Saldo Diário no MongoDB==*/

            foreach (MovimentacaoRealizada movimentacaoRealizada in movimentacoesRealizadas)
            {
                /*Lista todos os Saldos Diários da conta, com data >= à data de movimentação */
                foreach (var item in saldoDiarioDomainService.GetBySaldosDiario(movimentacaoRealizada.IdConta, movimentacaoRealizada.DataMovimentacaoRealizada))
                {
                    saldosDiario.Add(item);
                }


                /*adicionando no mongoDB..*/
                await mediator.Publish(new SaldoDiarioNotification
                {
                    SaldosDiario = saldosDiario,
                    Action = ActionNotification.Atualizar
                });               

            }
            
            return Unit.Value;
        }

    }
}