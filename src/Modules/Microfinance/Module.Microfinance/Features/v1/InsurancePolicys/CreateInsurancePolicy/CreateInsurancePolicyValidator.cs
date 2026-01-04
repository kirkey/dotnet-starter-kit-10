namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.CreateInsurancePolicy;

public class CreateInsurancePolicyValidator : AbstractValidator<CreateInsurancePolicyCommand>
{
    public CreateInsurancePolicyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
