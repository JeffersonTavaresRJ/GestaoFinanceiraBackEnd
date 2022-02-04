using FluentValidation;
using GestaoFinanceira.Domain.Models;

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
                .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty().WithMessage("O valor é obrigatório")
                .GreaterThanOrEqualTo(1).WithMessage("O Valor deve ser igual ou maior do que 1");

            RuleFor(mp => mp.NrParcela)
                .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty().WithMessage("O número da parcela é obrigatório")
                .GreaterThanOrEqualTo(1).WithMessage("O número da parcela deve ser igual ou maior do que 1");

            RuleFor(mp => mp.NrParcelaTotal)
                .Cascade(CascadeMode.Stop)//tratamento para encadear a execução das validações..
                .NotEmpty().WithMessage("O número total da parcela é obrigatório")
                .GreaterThanOrEqualTo(1).WithMessage("O total de parcelas deve ser igual ou maior do que 1");

            RuleFor(mp => mp.IdFormaPagamento)
                .NotEmpty().WithMessage("O id da forma de pagamento é obrigatório");

            RuleFor(mp => mp.Movimentacao.Observacao)
                .MaximumLength(100).WithMessage("A observação deve ter no máximo 100 caracteres");

            RuleFor(mp => mp.Status)
                .IsInEnum().WithMessage("O status é diferente de: 'A' (Aberto), 'Q' (Quitado) e 'N' (Não Aplicado)");         
                
        }
    }
}
