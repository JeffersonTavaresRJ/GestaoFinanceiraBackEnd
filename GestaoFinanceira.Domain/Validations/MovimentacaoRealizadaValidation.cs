using FluentValidation;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Validations
{
    public class MovimentacaoRealizadaValidation : AbstractValidator<MovimentacaoRealizada>
    {
        public MovimentacaoRealizadaValidation()
        {
            RuleFor(mr => mr.IdItemMovimentacao)
                .NotEmpty()
                .OverridePropertyName("Item de Movimentação:")
                .WithMessage("O item de movimentação é obrigatório");

            RuleFor(mr => mr.DataReferencia)
                .NotEmpty()
                .OverridePropertyName("Data de Referência:")
                .WithMessage("A data de referência é obrigatória");

            RuleFor(mr => mr.DataMovimentacaoRealizada)
                .NotEmpty()
                .OverridePropertyName("Data de Movimentação:")
                .WithMessage("A data de movimentação é obrigatória");

            RuleFor(mr => mr.Valor)
                .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty()
                .OverridePropertyName("Valor:")
                .WithMessage("O valor é obrigatório. ")
                .NotEqual(0)
                .WithMessage("O valor deve ser diferente de zero. ");

            RuleFor(mr => mr.IdFormaPagamento)
                .NotEmpty()
                .OverridePropertyName("Forma de Pagamento:")
                .WithMessage("A forma de pagamento é obrigatória");

            RuleFor(mr => mr.IdConta)
                .NotEmpty()
                .OverridePropertyName("Conta:")
                .WithMessage("A conta é obrigatória");
        }
    }
}
