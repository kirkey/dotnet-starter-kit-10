using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.CreateFeeDefinition;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.CreateFeeDefinition;

public class CreateFeeDefinitionValidator : AbstractValidator<CreateFeeDefinitionCommand>
{
    public CreateFeeDefinitionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
