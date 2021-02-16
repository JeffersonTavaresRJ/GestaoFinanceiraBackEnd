using GestaoFinanceira.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Validations
{
    public class UsuarioValidation :AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("O nome do usuário é obrigatório")
                .Length(6, 50).WithMessage("O nome do usuário deve ter entre 6 a 50 caracteres");

            RuleFor(u => u.EMail)
                .NotEmpty().WithMessage("O e-mail do usuário é obrigatório")
                .EmailAddress().WithMessage("O e-mail do usuário é inválido")
                .Length(10, 50).WithMessage("O e-mail do usuário deve ter entre 10 a 50 caracteres");

            RuleFor(u => u.Senha)
               .NotEmpty().WithMessage("A senha do usuário é obrigatório")
               .Length(8, 12).WithMessage("A senha do usuário deve ter no mínimo 8 e no máximo 12 caracteres");

        }
    }
}
