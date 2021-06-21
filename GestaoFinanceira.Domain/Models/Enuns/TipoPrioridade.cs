using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GestaoFinanceira.Domain.Models.Enuns
{
    public enum TipoPrioridade
    {
        [Description("Alta")]
        A,
        [Description("Média")]
        M,
        [Description("Baixa")]
        B
    }
}
