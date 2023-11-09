using FluentValidation;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Validations
{
    public class TransferenciaContaValidation : AbstractValidator<TransferenciaContas>
    {
        public TransferenciaContaValidation()
        {
            RuleFor(tc => tc.IdConta)
                .NotEmpty()
                .OverridePropertyName("Conta:").WithMessage("A conta de origem é obrigatório");

            RuleFor(tc => tc.IdContaDestino)
                .NotEmpty()
                .OverridePropertyName("Conta Destino:").WithMessage("A conta de destino é obrigatório");

            RuleFor(tc => tc.IdContaDestino)
                .NotEqual(tc=>tc.IdConta)
                .OverridePropertyName("Conta Destino:").WithMessage("A conta de origem deve ser diferente da conta de destino");

            RuleFor(mr => mr.DataMovimentacaoRealizada)
                .NotEmpty()
                .OverridePropertyName("Data da Movimentação:").WithMessage("A data de movimentação é obrigatória");

            RuleFor(mr => mr.Valor)
                .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty()
                .OverridePropertyName("Valor:")
                .WithMessage("O valor é obrigatório. ")
                .GreaterThan(0)
                .WithMessage("O valor deve ser maior do que zero. ");
        }
    }
}
