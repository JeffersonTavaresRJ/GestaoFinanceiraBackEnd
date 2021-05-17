using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class FormaPagamento
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public int IdUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
