﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Models
{
    public class Conta
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }
        public string DefaultConta { get; set; }
        public string Tipo { get; set; }
        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public virtual List<MovimentacaoRealizada> MovimentacoesRealizadas { get; set; }
        public virtual List<SaldoDiario> SaldosDiario { get; set; }

    }
}
