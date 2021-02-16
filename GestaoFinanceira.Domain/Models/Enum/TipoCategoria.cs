using System.ComponentModel;

namespace GestaoFinanceira.Domain.Models.Enuns
{

    public enum TipoCategoria
    {
        [Description("Despesa")]
        D,
        [Description("Receita")]
        R
    }

}