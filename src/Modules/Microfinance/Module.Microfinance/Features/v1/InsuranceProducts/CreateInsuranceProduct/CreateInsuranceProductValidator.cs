namespace FSH.Module.Microfinance.Features.v1.InsuranceProducts.CreateInsuranceProduct;

public class CreateInsuranceProductValidator : AbstractValidator<CreateInsuranceProductCommand>
{
    public CreateInsuranceProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
