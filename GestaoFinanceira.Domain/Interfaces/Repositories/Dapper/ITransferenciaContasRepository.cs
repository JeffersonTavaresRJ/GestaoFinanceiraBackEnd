﻿using GestaoFinanceira.Domain.Models;
using System.Collections.Generic;

namespace GestaoFinanceira.Domain.Interfaces.Repositories.Dapper
{
    public interface ITransferenciaContasRepository
    {
        IEnumerable<dynamic> Execute(TransferenciaContas transfereConta);
    }
}
