namespace FSH.Module.Microfinance.Features.v1.CashVaults.CreateCashVault;

public class CreateCashVaultValidator : AbstractValidator<CreateCashVaultCommand>
{
    public CreateCashVaultValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
