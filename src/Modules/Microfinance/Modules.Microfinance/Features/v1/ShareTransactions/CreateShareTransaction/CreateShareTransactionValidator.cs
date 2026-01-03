namespace FSH.Modules.Microfinance.Features.v1.ShareTransactions.CreateShareTransaction;

public class CreateShareTransactionValidator : AbstractValidator<CreateShareTransactionCommand>
{
    public CreateShareTransactionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
