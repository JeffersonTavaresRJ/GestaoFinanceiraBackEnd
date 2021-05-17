using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Application.Commands.FormaPagamento
{
    public class UpdateFormaPagamentoCommand : IRequest
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Status { get; set; }

        public int IdUsuario { get; set; }

    }
}
