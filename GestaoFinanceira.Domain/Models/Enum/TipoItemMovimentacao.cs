using System.ComponentModel;

namespace GestaoFinanceira.Domain.Models.Enuns
{

    public enum TipoItemMovimentacao
    {
        [Description("Despesa")]
        D,
        [Description("Receita")]
        R
    }

}