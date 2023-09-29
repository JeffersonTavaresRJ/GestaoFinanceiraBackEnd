using GestaoFinanceira.Domain.DTOs;
using GestaoFinanceira.Domain.Interfaces.Caching;
using GestaoFinanceira.Infra.CrossCutting.GenericFunctions;
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


        public List<MovimentacaoRealizadaMensalDTO> GetByMovimentacaoRealizadaMensal(DateTime dataReferencia)
        {
            var ano = dataReferencia.Year;
            var mes = dataReferencia.Month;

            //Movimentações Realizadas no período..
            List<MovimentacaoRealizadaDTO> movimentacaoRealizadaDTOs = 
                this.movimentacaoRealizadaCaching.GetByDataMovimentacaoRealizada(null, new DateTime(ano-1, mes, 1), new DateTime(ano, mes+1, 1).AddDays(-1));
            
            //Todas as Contas com movimentações no período..
            List<ContaDTO> contaDTOs = 
                movimentacaoRealizadaDTOs.Where(c=>c.Conta.Id.Equals(2015)).Select(mr=>mr.Conta).Distinct().ToList().OrderBy(c=>c.Descricao).ToList();

            //return da function..
            List<MovimentacaoRealizadaMensalDTO> movimentacaoRealizadaMensalDTOs = new List<MovimentacaoRealizadaMensalDTO>();          

            //Percorrendo as Contas..
            foreach (ContaDTO contaDTO in contaDTOs)
            {

                //Leitura de todos os Itens de Movimentação por Conta durante o período..
                List<ItemMovimentacaoDTO> itemMovimentacaoDTOs =
                movimentacaoRealizadaDTOs.Where(mr=>mr.Conta.Id.Equals(contaDTO.Id))
                                         .Select(mr => mr.ItemMovimentacao)
                                         .Distinct()
                                         .ToList()
                                         .OrderBy(c => c.Descricao)
                                         .ToList();

                //Calculando Saldos da Conta..
                List<SaldoContaDTO> saldoContaDTOs = new List<SaldoContaDTO>();

                var meses = 0;
                ano = dataReferencia.Year - 1;
                mes = dataReferencia.Month;

                while (meses <= 12)
                {
                    SaldoContaDTO saldoContaDTO = new SaldoContaDTO()
                    {
                        Mes = mes,
                        Ano = ano,
                        SaldoAnterior = saldoDiarioCaching.GetSaldoConta(contaDTO.Id, new DateTime(ano, mes, 1).AddMonths(-1)),
                        SaldoAtual = saldoDiarioCaching.GetSaldoConta(contaDTO.Id, new DateTime(ano, mes, 1))
                    };

                    mes++;
                    if (mes == 13)
                    {
                        mes = 1;
                        ano++;
                    }
                    saldoContaDTOs.Add(saldoContaDTO);

                    meses++;
                }


                MovimentacaoRealizadaMensalDTO movimentacaoRealizadaMensalDTO = 
                                               new MovimentacaoRealizadaMensalDTO(contaDTO, saldoContaDTOs, itemMovimentacaoDTOs);


                //Populando o valor mensal de cada item..

                foreach (TipoMovimentacao tipoMovimentacao in movimentacaoRealizadaMensalDTO.TiposMovimentacao)
                {

                    foreach (ItemDTO item in tipoMovimentacao.ItemDTOs)
                    {
                        meses = 0;
                        ano = dataReferencia.Year-1;
                        mes = dataReferencia.Month;
                        List<MesItemDTO> mesItemDTOs = new List<MesItemDTO>();

                        while (meses <= 12)
                        {
                            
                            MesItemDTO mesItemDTO = new MesItemDTO()
                            {
                                Mes = mes,
                                Ano = ano,
                                Valor = GetValor(contaDTO.Id, item.ItemMovimentacaoDTO.Id, ano, mes, movimentacaoRealizadaDTOs)
                            };

                            mes++;
                            if (mes == 13)
                            {
                                mes = 1;
                                ano++;
                            }
                            movimentacaoRealizadaMensalDTO.UpdateItemMovimentacao(item.ItemMovimentacaoDTO, mesItemDTO);
                            meses++;
                        }
                        
                    }                    
                    
                }

                movimentacaoRealizadaMensalDTOs.Add(movimentacaoRealizadaMensalDTO);
            }

            return movimentacaoRealizadaMensalDTOs;
        }

        
        private double GetValor(int idConta, int idItemMovimentacao, int ano, int mes, List<MovimentacaoRealizadaDTO> movimentacaoRealizadaDTOs)
        {
            var dataIni = new DateTime(ano, mes, 1);
            var dataFim = new DateTime(ano, mes, 1).AddMonths(1).AddDays(-1);

            return movimentacaoRealizadaDTOs.Where(mr=>mr.ItemMovimentacao.Id.Equals(idItemMovimentacao) &&
                                                       mr.Conta.Id.Equals(idConta) &&
                                                       mr.DataMovimentacaoRealizada >= DateTimeClass.DataHoraIni(dataIni) &&
                                                       mr.DataMovimentacaoRealizada <= DateTimeClass.DataHoraFim(dataFim))
                                            .Select(mr=>mr.Valor)
                                            .Sum();
        }
    }
}
