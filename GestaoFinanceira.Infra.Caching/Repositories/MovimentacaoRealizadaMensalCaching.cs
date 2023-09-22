using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.Caching.Context;
using GestaoFinanceira.Infra.CrossCutting.GenericFunctions;
using GestaoFinanceira.Infra.CrossCutting.Security;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Infra.Caching.Repositories
{
    public class MovimentacaoRealizadaMensalCaching :IMovimentacaoRealizadaMensalCaching
    {
        private readonly IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching;
        private readonly ISaldoDiarioCaching saldoDiarioCaching;

        public MovimentacaoRealizadaMensalCaching(IMovimentacaoRealizadaCaching movimentacaoRealizadaCaching, ISaldoDiarioCaching saldoDiarioCaching)
        {
            this.movimentacaoRealizadaCaching = movimentacaoRealizadaCaching;
            this.saldoDiarioCaching = saldoDiarioCaching;
        }


        public List<MovimentacaoRealizadaMensalDTO> GetByDataMovimentacaoRealizada(DateTime dataReferencia)
        {
            var ano = dataReferencia.Year;
            var mes = dataReferencia.Month;

            //Movimentações Realizadas no período..
            List<MovimentacaoRealizadaDTO> movimentacaoRealizadaDTOs = 
                this.movimentacaoRealizadaCaching.GetByDataMovimentacaoRealizada(null, new DateTime(ano-1, mes, 1), new DateTime(ano, mes+1, 1).AddDays(-1));
            
            //Todas as Contas com movimentações no período..
            List<ContaDTO> contaDTOs = 
                movimentacaoRealizadaDTOs.Select(mr=>mr.Conta).Distinct().ToList().OrderBy(c=>c.Descricao).ToList();

            List<MovimentacaoRealizadaMensalDTO> movimentacaoRealizadaMensalDTOs = new List<MovimentacaoRealizadaMensalDTO>();          

            //Percorrendo as Contas..
            foreach (ContaDTO contaDTO in contaDTOs)
            {

                //Todos os Itens de Movimentação por Conta durante o período..
                List<ItemMovimentacaoDTO> itemMovimentacaoDTOs =
                movimentacaoRealizadaDTOs.Where(mr=>mr.Conta.Id.Equals(contaDTO.Id))
                                         .Select(mr => mr.ItemMovimentacao)
                                         .Distinct()
                                         .ToList()
                                         .OrderBy(c => c.Descricao)
                                         .ToList();

                MovimentacaoRealizadaMensalDTO movimentacaoRealizadaMensalDTO = 
                                               new MovimentacaoRealizadaMensalDTO(contaDTO, itemMovimentacaoDTOs, dataReferencia);
                


                //Percorrendo os meses do período..
                foreach (SaldoContaDTO saldoContaDTO in movimentacaoRealizadaMensalDTO.SaldosContaDTO)
                {
                    //Calculando os Saldos..
                    saldoContaDTO.SaldoAnterior = GetSaldo(contaDTO.Id, new DateTime(saldoContaDTO.Ano, saldoContaDTO.Mes, 1).AddMonths(-1));
                    saldoContaDTO.SaldoAtual = GetSaldo(contaDTO.Id, new DateTime(saldoContaDTO.Ano, saldoContaDTO.Mes, 1));
                    movimentacaoRealizadaMensalDTO.UpdateSaldo(saldoContaDTO);

                    //Calculando os Valores dos Itens de Movimentação..
                    foreach (var item in itemMovimentacaoDTOs)
                    {
                        double valor = GetValor(contaDTO.Id, item.Id, saldoContaDTO.Ano, saldoContaDTO.Mes, movimentacaoRealizadaDTOs);
                        movimentacaoRealizadaMensalDTO.UpdateItemMovimentacao(item, saldoContaDTO.Ano, saldoContaDTO.Mes, valor);
                    }
                }               


                movimentacaoRealizadaMensalDTOs.Add(movimentacaoRealizadaMensalDTO);
            }

            return movimentacaoRealizadaMensalDTOs;
        }

        private double GetSaldo(int idConta, DateTime dataReferencia)
        {
            List<SaldoDiarioDTO> saldoAnteriorConta = saldoDiarioCaching.GetMaxGroupBySaldoConta(dataReferencia);

            return saldoAnteriorConta.Where(s => s.Conta.Id.Equals(idConta))
                                     .Select(s => s.Valor)
                                     .FirstOrDefault();
        }

        private double GetValor(int idConta, int idItemMovimentacao, int ano, int mes, List<MovimentacaoRealizadaDTO> movimentacaoRealizadaDTOs)
        {
            var dataIni = new DateTime(ano, mes, 1);
            var dataFim = new DateTime(ano, mes+1, 1).AddDays(-1);

            return movimentacaoRealizadaDTOs.Where(mr=>mr.ItemMovimentacao.Id.Equals(idItemMovimentacao) &&
                                                       mr.Conta.Id.Equals(idConta) &&
                                                       mr.DataMovimentacaoRealizada >= dataIni &&
                                                       mr.DataMovimentacaoRealizada <= dataFim )
                                            .Select(mr=>mr.Valor)
                                            .Sum();
        }
    }
}
