using FluentValidation;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Validations
{
    public class MovimentacaoPrevistaValidation : AbstractValidator<MovimentacaoPrevista>
    {
        public MovimentacaoPrevistaValidation()
        {
            RuleFor(mp => mp.IdItemMovimentacao)
                .NotEmpty().WithMessage("O id do item de movimentação é obrigatório");

            RuleFor(mp => mp.DataReferencia)
                .NotEmpty().WithMessage("A data de referência é obrigatória");

            RuleFor(mp => mp.DataVencimento)
                .NotEmpty().WithMessage("A data de vencimento é obrigatória");

            RuleFor(mp => mp.Valor)
                .NotEmpty().WithMessage("O valor é obrigatório")
                .GreaterThanOrEqualTo(1).WithMessage("O Valor deve ser igual ou maior do que 1");

            RuleFor(mp => mp.IdFormaPagamento)
                .NotEmpty().WithMessage("O id da forma de pagamento é obrigatório");

            RuleFor(mp => mp.Status)
                .IsInEnum().WithMessage("O status é diferente de: 'A' (Aberto), 'Q' (Quitado) e 'N' (Não Aplicado)");         
                
        }
    }
}
