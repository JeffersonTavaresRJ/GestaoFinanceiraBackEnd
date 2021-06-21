using GestaoFinanceira.Domain.Models.Enuns;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class ItemMovimentacao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public TipoItemMovimentacao Tipo { get; set; }
        public bool Status { get; set; }
        public int IdCategoria { get; set; }
        public virtual Categoria Categoria { get; set; }
        public virtual List<Movimentacao> Movimentacoes { get; set; }

        public ItemMovimentacao()
        {

        }
        
        public ItemMovimentacao(int id, string descricao, TipoItemMovimentacao tipo, bool status, int idCategoria)
        {
            Id = id;
            Descricao = descricao;
            Tipo = tipo;
            Status = status;
            IdCategoria = idCategoria;
        }
    }
}
