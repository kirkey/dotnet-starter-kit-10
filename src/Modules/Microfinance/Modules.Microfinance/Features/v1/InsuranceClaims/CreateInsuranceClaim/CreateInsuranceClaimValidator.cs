namespace FSH.Modules.Microfinance.Features.v1.InsuranceClaims.CreateInsuranceClaim;

public class CreateInsuranceClaimValidator : AbstractValidator<CreateInsuranceClaimCommand>
{
    public CreateInsuranceClaimValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
