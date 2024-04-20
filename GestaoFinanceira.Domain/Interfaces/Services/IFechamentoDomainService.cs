using GestaoFinanceira.Domain.DTOs;
using System;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IFechamentoDomainService
    {
        void Executar(int idUsuario, DateTime dataReferencia, string status);
        List<FechamentoMensalDTO> GetAll();
    }
}
