using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GestaoFinanceira.Domain.Models.Enuns
{
    public enum TipoRecorrencia
    {
        [Description("Nenhum")]
        N,
        [Description("Mensal")]
        M,
        [Description("Parcelado")]
        P
    }
}
