﻿using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Repositories
{
    public interface IFormaPagamentoRepository : IGenericRepository<FormaPagamento>
    {
        void Delete(int idUsuario);
    }
}