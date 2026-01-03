namespace FSH.Modules.Microfinance.Features.v1.InvestmentTransactions.CreateInvestmentTransaction;

public class CreateInvestmentTransactionValidator : AbstractValidator<CreateInvestmentTransactionCommand>
{
    public CreateInvestmentTransactionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
