using FSH.Module.Microfinance.Contracts.v1.ShareTransactions.CreateShareTransaction;

namespace FSH.Module.Microfinance.Features.v1.ShareTransactions.CreateShareTransaction;

public class CreateShareTransactionValidator : AbstractValidator<CreateShareTransactionCommand>
{
    public CreateShareTransactionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
