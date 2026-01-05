using FSH.Module.Microfinance.Contracts.v1.MobileTransactions.CreateMobileTransaction;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.CreateMobileTransaction;

public class CreateMobileTransactionValidator : AbstractValidator<CreateMobileTransactionCommand>
{
    public CreateMobileTransactionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
