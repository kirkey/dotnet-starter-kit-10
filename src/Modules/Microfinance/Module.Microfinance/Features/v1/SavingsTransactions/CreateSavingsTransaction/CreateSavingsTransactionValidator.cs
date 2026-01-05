using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.CreateSavingsTransaction;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.CreateSavingsTransaction;

public class CreateSavingsTransactionValidator : AbstractValidator<CreateSavingsTransactionCommand>
{
    public CreateSavingsTransactionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
