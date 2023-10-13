using FluentValidation;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Validations
{
    public class TransferenciaContaValidation : AbstractValidator<TransferenciaContas>
    {
        public TransferenciaContaValidation()
        {
            RuleFor(tc => tc.IdConta)
                .NotEmpty().WithMessage("A conta de origem é obrigatório");

            RuleFor(tc => tc.IdContaDestino)
                .NotEmpty().WithMessage("A conta de destino é obrigatório");

            RuleFor(tc => tc.IdConta)
                .NotEqual(tc=>tc.IdContaDestino).WithMessage("A conta de origem deve ser diferente da conta de destino");

            RuleFor(mr => mr.DataMovimentacaoRealizada)
                .NotEmpty().WithMessage("A data de movimentação é obrigatória");

            RuleFor(mr => mr.Valor)
                .NotEmpty().WithMessage("O valor é obrigatório. ")
                .GreaterThan(0).WithMessage("O valor deve ser maior do que zero. ");
        }
    }
}
