namespace FSH.Module.Microfinance.Features.v1.FeeCharges.CreateFeeCharge;

public class CreateFeeChargeValidator : AbstractValidator<CreateFeeChargeCommand>
{
    public CreateFeeChargeValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
