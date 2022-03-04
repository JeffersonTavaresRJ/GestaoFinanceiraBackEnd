using AutoMapper;
using FluentValidation;
using GestaoFinanceira.Application.Commands.MovimentacaoRealizada;
using GestaoFinanceira.Application.Notifications;
using GestaoFinanceira.Domain.Exceptions.MovimentacaoPrevista;
using GestaoFinanceira.Domain.Interfaces.Services;
using GestaoFinanceira.Domain.Models;
using GestaoFinanceira.Domain.Validations;
using MediatR;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GestaoFinanceira.Application.RequestHandler
{


    public class MovimentacaoRealizadaRequestHandler : IRequestHandler<CreateMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<UpdateMovimentacaoRealizadaCommand>,
                                                       IRequestHandler<DeleteMovimentacaoRealizadaCommand>
    {
        private readonly IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService;
        private readonly IMovimentacaoDomainService movimentacaoDomainService;
        private readonly ISaldoDiarioDomainService saldoDiarioDomainService;
        private readonly IMediator mediator;
        private readonly IMapper mapper;
        private List<MovimentacaoPrevista> movimentacoesPrevistas;
        private MovimentacaoPrevista movimentacaoPrevista;
        private List<SaldoDiario> saldosDiario = new List<SaldoDiario>();


        public MovimentacaoRealizadaRequestHandler(IMovimentacaoRealizadaDomainService movimentacaoRealizadaDomainService, 
                                                   IMovimentacaoDomainService movimentacaoDomainService,
                                                   ISaldoDiarioDomainService saldoDiarioDomainService,
                                                   IMediator mediator, 
                                                   IMapper mapper)
        {
            this.movimentacaoRealizadaDomainService = movimentacaoRealizadaDomainService;
            this.movimentacaoDomainService = movimentacaoDomainService;
            this.saldoDiarioDomainService = saldoDiarioDomainService;
            this.mediator = mediator;
            this.mapper = mapper; 
        }

        public async Task<Unit> Handle(CreateMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            List<MovimentacaoRealizada> movimentacoesRealizadas = new List<MovimentacaoRealizada>();

            foreach (MovimentacaoRealizadaCommand item in request.MovimentacaoRealizadaCommand)
            {
                MovimentacaoRealizada movimentacaoRealizada = mapper.Map<MovimentacaoRealizada>(item);

                var validate = new MovimentacaoRealizadaValidation().Validate(movimentacaoRealizada);
                if (!validate.IsValid)
                {
                    throw new ValidationException(validate.Errors);
                }

                movimentacoesRealizadas.Add(movimentacaoRealizada);
                //adicionando Saldos Diário para pesquisa..
                this.saldosDiario.Add(new SaldoDiario { IdConta = movimentacaoRealizada.IdConta, DataSaldo = movimentacaoRealizada.DataMovimentacaoRealizada});

            }
            /*adicionando no banco de dados..*/
            movimentacoesRealizadas = movimentacaoRealizadaDomainService.Add(movimentacoesRealizadas, out movimentacoesPrevistas);

            /*adicionando no mongoDB..*/
            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacoesRealizadas = movimentacoesRealizadas,
                Action = ActionNotification.Criar
            });


            /*==Atualização do Saldo Diário no MongoDB==*/

            /*agrupando por Conta com a menor Data de Saldo..*/
            dynamic saldoGroup = new List<dynamic>();
            saldoGroup.Add(new ExpandoObject());

            saldoGroup = saldosDiario.GroupBy(sa => sa.IdConta)
                        .Select(g => new { IdConta = g.Key, 
                                           DataSaldo = g.Min(row => row.DataSaldo) 
                         });

            /*adicionando no mongoDB..*/
            foreach (var item in saldoGroup)
            {
                saldosDiario.Clear();
                saldosDiario = saldoDiarioDomainService.GetBySaldosDiario(item.IdConta, item.DataSaldo);
                
                await mediator.Publish(new SaldoDiarioNotification
                {
                    SaldosDiario = saldosDiario,
                    Action = ActionNotification.Criar
                });
            }

            /*==Atualização do Status das Movimentações Previstas no MongoDB==*/

            List<string> messages = new List<string>();
            foreach (var movimentacaoPrevista in movimentacoesPrevistas)
            {
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacaoPrevista = movimentacaoPrevista,
                    Action = ActionNotification.Atualizar
                });

                /*movimentações previstas quitadas como mensagem para API..*/
                messages.Add(new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status).Message);

            }

            /*Notificação das Movimentações Previstas com Status alterado..*/
            if (movimentacoesPrevistas.Count > 0)
            {
                throw new MovPrevAlteraStatus(messages.Distinct().ToList());
            }

            return Unit.Value;
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

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacaoRealizada = movimentacaoRealizada,
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
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacaoPrevista = movimentacaoPrevista,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status);
            }

            return Unit.Value;
        }

        public async Task<Unit> Handle(DeleteMovimentacaoRealizadaCommand request, CancellationToken cancellationToken)
        {
            MovimentacaoRealizada movimentacaoRealizada = movimentacaoRealizadaDomainService.GetId(request.Id);
            movimentacaoRealizadaDomainService.Delete(movimentacaoRealizada, out movimentacaoPrevista);

            await mediator.Publish(new MovimentacaoRealizadaNotification
            {
                MovimentacaoRealizada = movimentacaoRealizada,
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
                await mediator.Publish(new MovimentacaoPrevistaNotification
                {
                    MovimentacaoPrevista = movimentacaoPrevista,
                    Action = ActionNotification.Atualizar
                });

                throw new MovPrevAlteraStatus(movimentacaoPrevista.Movimentacao.ItemMovimentacao.Descricao,
                                                     movimentacaoPrevista.DataReferencia,
                                                     movimentacaoPrevista.Status);
            }

            return Unit.Value;
        }
    }
}