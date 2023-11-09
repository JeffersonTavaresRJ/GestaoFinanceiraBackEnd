using GestaoFinanceira.Domain.Models;
using FluentValidation;

namespace GestaoFinanceira.Domain.Validations
{
    public class CategoriaValidation : AbstractValidator<Categoria>
    {
        public CategoriaValidation()
        {

            RuleFor(c => c.IdUsuario)
                .NotEmpty()
                .OverridePropertyName("Usuário:")
                .WithMessage("O usuário da categoria é obrigatório");

            RuleFor(c => c.Descricao)
                .NotEmpty()
                .OverridePropertyName("Descrição:")
                .WithMessage("A descrição da categoria é obrigatório")
                .Length(4, 50)
                .WithMessage("A descrição da categoria deve ter entre 4 a 50 caracteres");

        }
    }
}
