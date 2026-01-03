namespace FSH.Modules.Microfinance.Features.v1.InvestmentAccounts.CreateInvestmentAccount;

public class CreateInvestmentAccountValidator : AbstractValidator<CreateInvestmentAccountCommand>
{
    public CreateInvestmentAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
