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
                .NotEmpty()
                .OverridePropertyName("Categoria:")
                .WithMessage("A categoria deve ser informada");
            
            RuleFor(i => i.Descricao)
                .NotEmpty()
                .OverridePropertyName("Descrição:")
                .WithMessage("A descrição é obrigatória")
                .Length(4, 50)
                .WithMessage("A descrição deve possuir entre 4 a 50 caracteres");

            RuleFor(i => i.Tipo)
                .IsInEnum()
                .OverridePropertyName("Tipo:")
                .WithMessage("O tipo é diferente de: 'D' (Despesa) e 'R' (Receita)");
            
        }
    }
}
