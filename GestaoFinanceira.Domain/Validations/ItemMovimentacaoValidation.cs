using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Validations
{
    public class ItemMovimentacaoValidation: AbstractValidator<ItemMovimentacao>
    {
        public ItemMovimentacaoValidation()
        {
            RuleFor(i => i.IdCategoria)
                .NotEmpty().WithMessage("O Id da categoria deve ser informado");
            
            RuleFor(i => i.Descricao)
                .NotEmpty().WithMessage("A descrição é obrigatória")
                .Length(4, 50).WithMessage("A descrição deve possuir entre 4 a 50 caracteres");

            RuleFor(i => i.Tipo)
                .IsInEnum().WithMessage("O tipo é diferente de: 'D' (Despesa) e 'R' (Receita)");
            
        }
    }
}
