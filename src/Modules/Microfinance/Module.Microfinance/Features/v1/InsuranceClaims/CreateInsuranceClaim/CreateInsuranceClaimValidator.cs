using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.CreateInsuranceClaim;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.CreateInsuranceClaim;

public class CreateInsuranceClaimValidator : AbstractValidator<CreateInsuranceClaimCommand>
{
    public CreateInsuranceClaimValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
