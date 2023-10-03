using System.Collections.Generic;
using System.Linq;

namespace GestaoFinanceira.Domain.DTOs
{
    public class MovimentacaoRealizadaMensalDTO
    {

        public MovimentacaoRealizadaMensalDTO(ContaDTO conta, List<SaldoContaDTO> saldoContaDTOs, List<ItemMovimentacaoDTO> itemMovimentacaoDTOs)
        {
            Conta = conta;
            SaldoContaDTOs = saldoContaDTOs;
            TiposMovimentacao = PopulaTiposMovimentacao(itemMovimentacaoDTOs);
        }


        public ContaDTO Conta { get; set; }
        public List<SaldoContaDTO> SaldoContaDTOs { get; set; }
        public List<TipoMovimentacao> TiposMovimentacao { get; set; }

        public void UpdateItemMovimentacao(ItemMovimentacaoDTO itemMovimentacaoDTO, MesItemDTO mesItemDTO)
        {
            this.TiposMovimentacao.Find(t => t.Tipo.Equals(itemMovimentacaoDTO.Tipo))
                                  .ItemDTOs.Find(i => i.ItemMovimentacaoDTO.Id.Equals(itemMovimentacaoDTO.Id)).Meses.Add(mesItemDTO);            
        }

        private static List<TipoMovimentacao> PopulaTiposMovimentacao(List<ItemMovimentacaoDTO> itemMovimentacaoDTOs)
        {
            List<TipoMovimentacao> tiposMovimentacao = new List<TipoMovimentacao>();

            List<ItemMovimentacaoDTO> itensReceita = itemMovimentacaoDTOs.Where(it => it.Tipo.Equals("R")).OrderBy(it => it.Descricao).ToList();
            List<ItemMovimentacaoDTO> itensDespesa = itemMovimentacaoDTOs.Where(it => it.Tipo.Equals("D")).OrderBy(it => it.Descricao).ToList();

            TipoMovimentacao tipoMovimentacaoRec = new TipoMovimentacao("R");
            TipoMovimentacao tipoMovimentacaoDes = new TipoMovimentacao("D");

            foreach (ItemMovimentacaoDTO itemMovimentacaoDTO in itensReceita)
            {
                ItemDTO itemDTO = new ItemDTO(itemMovimentacaoDTO);
                tipoMovimentacaoRec.ItemDTOs.Add(itemDTO);                
            }

            
            foreach (ItemMovimentacaoDTO itemMovimentacaoDTO in itensDespesa)
            {
                ItemDTO itemDTO = new ItemDTO(itemMovimentacaoDTO);
                tipoMovimentacaoDes.ItemDTOs.Add(itemDTO);               
            }

            tiposMovimentacao.Add(tipoMovimentacaoRec);
            tiposMovimentacao.Add(tipoMovimentacaoDes);
            
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
            ItemDTOs = new List<ItemDTO>();
        }

        public string Tipo { get; set; }
        public List<ItemDTO> ItemDTOs { get; set; }

    }

    public class ItemDTO
    {
        public ItemDTO(ItemMovimentacaoDTO itemMovimentacaoDTO)
        {
            ItemMovimentacaoDTO = itemMovimentacaoDTO;
            Meses = new List<MesItemDTO>();
        }

        public ItemMovimentacaoDTO ItemMovimentacaoDTO { get; set; }
        public List<MesItemDTO> Meses { get; set; }
    }

    public class MesItemDTO
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public double Valor { get; set; }
    }

}
