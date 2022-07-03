using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Interfaces
{
    public interface IFechamentoApplicationService
    {
        void Executar(int idUsuario, DateTime dataReferencia);
    }
}
