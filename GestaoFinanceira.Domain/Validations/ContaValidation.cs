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
            RuleFor(c => c.IdUsuario)
                .NotEmpty()
                .OverridePropertyName("Usuário:")
                .WithMessage("O usuário da conta é obrigatório");

            RuleFor(c => c.Descricao)
                .NotEmpty()
                .OverridePropertyName("Descrição:")
                .WithMessage("A descrição da conta é obrigatório")
                .Length(5, 50)
                .WithMessage("A descrição da conta deve ter entre 5 a 50 caracteres");
           
        }
    }
}
