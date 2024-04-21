using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.Dapper
{
    public interface IFechamentoRepository
    {
        void Executar(int idUsuario, DateTime dataReferencia, string status);
        List<FechamentoMensalDTO> GetAll();
    }
}
