using FluentValidation;
using GestaoFinanceira.Domain.Models;

namespace GestaoFinanceira.Domain.Validations
{
    public class MovimentacaoPrevistaValidation : AbstractValidator<MovimentacaoPrevista>
    {
        public MovimentacaoPrevistaValidation()
        {
            RuleFor(mp => mp.IdItemMovimentacao)
                .NotEmpty()
                .OverridePropertyName("Item Movimentação:")
                .WithMessage("O item de movimentação é obrigatório");

            RuleFor(mp => mp.DataReferencia)
                .NotEmpty()
                .OverridePropertyName("Data de Referência:")
                .WithMessage("A data de referência é obrigatória");

            RuleFor(mp => mp.DataVencimento)
                .NotEmpty()
                .OverridePropertyName("Data de Vencimento:")
                .WithMessage("A data de vencimento é obrigatória");

            RuleFor(mp => mp.Valor)
                .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty()
                .OverridePropertyName("Valor:")
                .WithMessage("O valor é obrigatório.")
                .GreaterThan(0)
                .OverridePropertyName("Valor:")
                .WithMessage("O valor deve ser maior do que zero.");

            RuleFor(mp => mp.NrParcela)
               .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty()
                .OverridePropertyName("Parcela:")
                .WithMessage("O número da parcela é obrigatório.")
                .GreaterThanOrEqualTo(1)
                .OverridePropertyName("Parcela:")
                .WithMessage("O número da parcela deve ser igual ou maior do que 1.");

            RuleFor(mp => mp.NrParcelaTotal)
               .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty()
                .OverridePropertyName("Total de Parcelas:")
                .WithMessage("O número total de parcelas é obrigatório.")
                .GreaterThanOrEqualTo(1)
                .OverridePropertyName("Total de Parcelas:")
                .WithMessage("O total de parcelas deve ser igual ou maior do que 1.");

            RuleFor(mp => mp.IdFormaPagamento)
                .NotEmpty()
                .OverridePropertyName("Forma de Pagamento:")
                .WithMessage("A forma de pagamento é obrigatória.");

            RuleFor(mp => mp.Observacao)
                .MaximumLength(200)
                .OverridePropertyName("Observação:")
                .WithMessage("A observação deve ter no máximo 200 caracteres.");

            RuleFor(mp => mp.Status)
                .IsInEnum()
                .OverridePropertyName("Status:")
                .WithMessage("O status é diferente de: 'A' (Aberto), 'Q' (Quitado) e 'N' (Não Aplicado).");         
                
        }
    }
}
