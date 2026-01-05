using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoPrevistaDomainService
    {
        List<MovimentacaoPrevista> AddResult(List<MovimentacaoPrevista> movimentacaoPrevistas);
        MovimentacaoPrevista UpdateResult(MovimentacaoPrevista movimentacaoPrevista);
        void Delete(MovimentacaoPrevista movimentacaoPrevista, out List<MovimentacaoPrevista> movimentacoesPrevistas);
        MovimentacaoPrevista GetByKey(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idItemMovimentacao, DateTime dataRefIni, DateTime dataRefFim);
    }
}
