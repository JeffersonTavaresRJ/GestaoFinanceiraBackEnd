using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Caching
{
    public interface IMovimentacaoRealizadaCaching: IGenericWriteCaching<MovimentacaoRealizadaDTO>
    {
        MovimentacaoRealizadaDTO GetId(int id);
        List<MovimentacaoRealizadaDTO> GetByDataReferencia(int idItemMovimentacao, DateTime dataReferencia);
        List<MovimentacaoRealizadaDTO> GetByDataMovimentacaoRealizada(int? idItemMovimentacao, int idUsuario, DateTime dataMovRealIni, DateTime dataMovRealFim);
    }
}
