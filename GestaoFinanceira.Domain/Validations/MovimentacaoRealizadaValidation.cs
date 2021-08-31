using FluentValidation;
using GestaoFinanceira.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestaoFinanceira.Domain.Validations
{
    public class MovimentacaoRealizadaValidation : AbstractValidator<MovimentacaoRealizada>
    {
        public MovimentacaoRealizadaValidation()
        {
            RuleFor(mr => mr.IdItemMovimentacao)
                .NotEmpty().WithMessage("O item de movimentação é obrigatório");

            RuleFor(mr => mr.DataReferencia)
                .NotEmpty().WithMessage("A data de referência é obrigatória");

            RuleFor(mr => mr.DataMovimentacaoRealizada)
                .NotEmpty().WithMessage("A data de movimentação é obrigatória");

            RuleFor(mr => mr.Valor)
                .NotEmpty().WithMessage("O valor é obrigatório")
                .GreaterThanOrEqualTo(1).WithMessage("O valor deve ser igual ou maior do que 1");

            RuleFor(mr => mr.IdFormaPagamento)
                .NotEmpty().WithMessage("A forma de pagamento é obrigatória");

            RuleFor(mr => mr.IdConta)
                .NotEmpty().WithMessage("A conta é obrigatória");
        }
    }
}
