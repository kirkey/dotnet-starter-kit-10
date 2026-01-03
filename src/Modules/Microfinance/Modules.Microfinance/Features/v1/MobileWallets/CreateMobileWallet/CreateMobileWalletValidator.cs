namespace FSH.Modules.Microfinance.Features.v1.MobileWallets.CreateMobileWallet;

public class CreateMobileWalletValidator : AbstractValidator<CreateMobileWalletCommand>
{
    public CreateMobileWalletValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
