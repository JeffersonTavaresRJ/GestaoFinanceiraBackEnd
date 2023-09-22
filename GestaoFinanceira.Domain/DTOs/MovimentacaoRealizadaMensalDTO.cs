using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace GestaoFinanceira.Domain.DTOs
{
    public class MovimentacaoRealizadaMensalDTO
    {

        public MovimentacaoRealizadaMensalDTO(ContaDTO conta, List<ItemMovimentacaoDTO> itemMovimentacaoDTOs, DateTime dataReferencia)
        {
            Conta = conta;
            SaldosContaDTO = PopulaSaldoContaDTO(dataReferencia);
            TiposMovimentacao = PopulaTiposMovimentacao(itemMovimentacaoDTOs, dataReferencia);
        }


        public ContaDTO Conta { get; set; }
        public List<SaldoContaDTO> SaldosContaDTO { get; set; }
        public List<TipoMovimentacao> TiposMovimentacao { get; set; }

        public void UpdateSaldo(SaldoContaDTO saldoContaDTO)
        {
            this.SaldosContaDTO.RemoveAll(x => x.Mes.Equals(saldoContaDTO.Mes) && x.Ano.Equals(saldoContaDTO.Ano));
            this.SaldosContaDTO.Add(saldoContaDTO);                              
        }

        public void UpdateItemMovimentacao(ItemMovimentacaoDTO itemMovimentacaoDTO, int ano, int mes, double valor)
        {

        }

        private List<SaldoContaDTO> PopulaSaldoContaDTO(DateTime dataReferencia)
        {
            List<SaldoContaDTO> lista = new List<SaldoContaDTO>();
            var ano = dataReferencia.Year;
            var mes = dataReferencia.Month;
            var meses = 13;

            while (meses > 0)
            {
                SaldoContaDTO saldoContaDTO = new SaldoContaDTO()
                {
                    Mes = mes,
                    Ano = ano,
                    SaldoAnterior = 0,
                    SaldoAtual = 0
                };

                mes = mes-- == 0 ? 12 : mes--;
                ano = mes-- == 0 ? ano-- : ano;

                lista.Add(saldoContaDTO);

                meses--;
            }

            return lista;
        }
        private static List<TipoMovimentacao> PopulaTiposMovimentacao(List<ItemMovimentacaoDTO> itemMovimentacaoDTOs, DateTime dataReferencia)
        {
            List<TipoMovimentacao> tiposMovimentacao = new List<TipoMovimentacao>();
            tiposMovimentacao.Add(new TipoMovimentacao("R"));
            tiposMovimentacao.Add(new TipoMovimentacao("D"));

            var itensReceita = itemMovimentacaoDTOs.Where(it => it.Tipo.Equals("R")).OrderBy(it => it.Descricao);
            var itensDespesa = itemMovimentacaoDTOs.Where(it => it.Tipo.Equals("D")).OrderBy(it => it.Descricao);

            foreach (TipoMovimentacao tipoMovimentacao in tiposMovimentacao)
            {
                if (tipoMovimentacao.Tipo.Equals("R"))
                {
                    foreach (var itemMovimentacaoDTO in itensReceita)
                    {
                        ItemDTO itemDTO = new ItemDTO(itemMovimentacaoDTO, dataReferencia);
                        tipoMovimentacao.ItemDTOs.Add(itemDTO);
                    }

                }

                if (tipoMovimentacao.Tipo.Equals("D"))
                {
                    foreach (var itemMovimentacaoDTO in itensDespesa)
                    {
                        ItemDTO itemDTO = new ItemDTO(itemMovimentacaoDTO, dataReferencia);
                        tipoMovimentacao.ItemDTOs.Add(itemDTO);
                    }

                }
            }
            return tiposMovimentacao;
        }

               
    }

    public class SaldoContaDTO
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double SaldoAnterior { get; set; }
        public double SaldoAtual { get; set; }
    }

    public class TipoMovimentacao
    {
        public TipoMovimentacao(string tipo)
        {
            Tipo = tipo;
        }

        public string Tipo { get; set; }
        public List<ItemDTO> ItemDTOs { get; set; }

    }

    public class ItemDTO
    {
        public ItemDTO(ItemMovimentacaoDTO itemMovimentacaoDTO, DateTime dataReferencia)
        {
            ItemMovimentacaoDTO = itemMovimentacaoDTO;
            Meses = PopulaMeses(dataReferencia);
        }

        public ItemMovimentacaoDTO ItemMovimentacaoDTO { get; set; }
        public List<MesItemDTO> Meses { get; set; }
        public List<MesItemDTO> PopulaMeses(DateTime dataReferencia)
        {
            List<MesItemDTO> lista = new List<MesItemDTO>();
            var ano = dataReferencia.Year;
            var mes = dataReferencia.Month;
            var meses = 13;

            while (meses > 0)
            {
                MesItemDTO mesItemDTO = new MesItemDTO()
                {
                    Mes = mes,
                    Ano = ano,
                    Valor = 0
                };

                mes = mes-- == 0 ? 12 : mes--;
                ano = mes-- == 0 ? ano-- : ano;

                lista.Add(mesItemDTO);

                meses--;
            }
            return lista;
        }
    }

    public class MesItemDTO
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double Valor { get; set; }
    }

}
