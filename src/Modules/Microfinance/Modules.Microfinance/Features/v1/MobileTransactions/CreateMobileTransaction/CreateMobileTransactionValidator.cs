namespace FSH.Modules.Microfinance.Features.v1.MobileTransactions.CreateMobileTransaction;

public class CreateMobileTransactionValidator : AbstractValidator<CreateMobileTransactionCommand>
{
    public CreateMobileTransactionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
