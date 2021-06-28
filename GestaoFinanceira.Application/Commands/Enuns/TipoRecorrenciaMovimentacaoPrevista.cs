using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GestaoFinanceira.Application.Commands.Enuns
{
    public enum TipoRecorrenciaMovimentacaoPrevista
    {
        [Description("Nenhum")]
        N,
        [Description("Mensal")]
        M,
        [Description("Parcelado")]
        P
    }
}
