using FSH.Module.Microfinance.Contracts.v1.RiskCategorys.CreateRiskCategory;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.CreateRiskCategory;

public class CreateRiskCategoryValidator : AbstractValidator<CreateRiskCategoryCommand>
{
    public CreateRiskCategoryValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
