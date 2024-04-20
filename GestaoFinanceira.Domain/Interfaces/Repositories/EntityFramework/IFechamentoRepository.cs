using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.EntityFramework
{
    public interface IFechamentoRepository
    {
        void Executar(int idUsuario, DateTime dataReferencia, string status);
        List<FechamentoMensalDTO> GetAll();
    }
}
