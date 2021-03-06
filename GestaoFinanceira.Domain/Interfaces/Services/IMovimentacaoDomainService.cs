﻿using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Interfaces.Services
{
    public interface IMovimentacaoDomainService
    {
        Movimentacao GetByKey(int idItemMovimentacao, DateTime dataReferencia);
    }
}
