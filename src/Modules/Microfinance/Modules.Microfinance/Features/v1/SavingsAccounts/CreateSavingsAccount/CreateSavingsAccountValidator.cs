namespace FSH.Modules.Microfinance.Features.v1.SavingsAccounts.CreateSavingsAccount;

public class CreateSavingsAccountValidator : AbstractValidator<CreateSavingsAccountCommand>
{
    public CreateSavingsAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
