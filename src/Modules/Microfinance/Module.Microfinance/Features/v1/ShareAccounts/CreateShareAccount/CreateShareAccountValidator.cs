namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.CreateShareAccount;

public class CreateShareAccountValidator : AbstractValidator<CreateShareAccountCommand>
{
    public CreateShareAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
