namespace FSH.Modules.Microfinance.Features.v1.FixedDeposits.CreateFixedDeposit;

public class CreateFixedDepositValidator : AbstractValidator<CreateFixedDepositCommand>
{
    public CreateFixedDepositValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
