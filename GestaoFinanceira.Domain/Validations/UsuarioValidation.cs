using GestaoFinanceira.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Validations
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(u => u.Nome).Length(6, 50).When(u => !string.IsNullOrEmpty(u.Nome))
                                              .WithMessage("O nome do usuário deve ter entre 6 a 50 caracteres")
                                //deve ficar por último para que a validação do valor nulo tenha efeito..
                                .NotEmpty().WithMessage("O nome do usuário é obrigatório");

            RuleFor(u => u.EMail)
                .EmailAddress().When(u => !string.IsNullOrEmpty(u.EMail))
                               .WithMessage("O e-mail do usuário é inválido")
                .Length(10, 50).When(u => !string.IsNullOrEmpty(u.EMail))
                               .WithMessage("O e-mail do usuário deve ter entre 10 a 50 caracteres")
                //deve ficar por último para que a validação do valor nulo tenha efeito..
                .NotEmpty().WithMessage("O e-mail do usuário é obrigatório");

            RuleFor(u => u.Senha)
               .Length(8, 12).When(u => !string.IsNullOrEmpty(u.Senha))
                             .WithMessage("A senha do usuário deve ter no mínimo 8 e no máximo 12 caracteres")
               //deve ficar por último para que a validação do valor nulo tenha efeito..
               .NotEmpty().WithMessage("A senha do usuário é obrigatório");


        }
    }
}
