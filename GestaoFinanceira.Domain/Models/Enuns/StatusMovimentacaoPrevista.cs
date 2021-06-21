using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GestaoFinanceira.Domain.Models.Enuns
{
    public enum StatusMovimentacaoPrevista
    {
        [Description("Aberto")]
        A,
        [Description("Quitado")]
        Q
    }
}
