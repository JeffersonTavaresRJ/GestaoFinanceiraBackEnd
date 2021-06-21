﻿using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IMovimentacaoPrevistaRepository : IGenericRepository<MovimentacaoPrevista>
    {
        IEnumerable<MovimentacaoPrevista> GetByDataReferencia(int idUsuario, int? idItemMovimentacao, DateTime dataReferenciaInicial, DateTime dataReferenciaFinal);
    }
}
