using FluentValidation;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Validations
{
    public class ContaValidation : AbstractValidator<Conta>
    {
        public ContaValidation()
        {
            RuleFor(c => c.IdUsuario).NotEmpty().WithMessage("O Id do usuário da conta é obrigatório");

            RuleFor(c => c.Descricao).NotEmpty().WithMessage("A descrição da conta é obrigatório")
                .Length(5, 50).WithMessage("A descrição da conta deve ter entre 5 a 50 caracteres"); 
        }
    }
}
