using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoPrevistaDomainService : IGenericWriteDomainService<MovimentacaoPrevista>
    {
        void Add(MovimentacaoPrevista movimentacaoPrevista, int qtdeParcelas);
        MovimentacaoPrevista GetByKey(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idItemMovimentacao, DateTime dataRefIni, DateTime dataRefFim);
    }
}
