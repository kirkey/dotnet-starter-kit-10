namespace FSH.Modules.Microfinance.Features.v1.DebtSettlements.CreateDebtSettlement;

public class CreateDebtSettlementValidator : AbstractValidator<CreateDebtSettlementCommand>
{
    public CreateDebtSettlementValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
