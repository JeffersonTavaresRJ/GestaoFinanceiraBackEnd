using FluentValidation;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Validations
{
    public class FormaPagamentoValidation : AbstractValidator<FormaPagamento>
    {
        public FormaPagamentoValidation()
        {
            RuleFor(f => f.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória")
                .Length(4, 50).WithMessage("A descrição deve ter entre 4 a 50 caracteres");

            RuleFor(f => f.IdUsuario)
                .NotEmpty().WithMessage("O Id do usuário é obrigatório");
        }
    }
}
