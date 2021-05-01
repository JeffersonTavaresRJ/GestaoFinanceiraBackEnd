using GestaoFinanceira.Domain.Models;
using FluentValidation;

namespace GestaoFinanceira.Domain.Validations
{
    public class CategoriaValidation : AbstractValidator<Categoria>
    {
        public CategoriaValidation()
        {
            /*  RuleFor(c => c.Tipo)
                  .IsInEnum().WithMessage("O tipo de categoria é diferente de: D-Despesa e R-Receita");
            */

            RuleFor(c => c.IdUsuario)
                .NotEmpty().WithMessage("O Id do usuário da categoria é obrigatório");

            RuleFor(c => c.Descricao)
                .NotEmpty().WithMessage("A descrição da categoria é obrigatório")
                .Length(5, 50).WithMessage("A descrição da categoria deve ter entre 5 a 50 caracteres");          
            
        }
    }
}
