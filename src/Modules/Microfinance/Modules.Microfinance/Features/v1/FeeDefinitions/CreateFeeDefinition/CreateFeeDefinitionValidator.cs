namespace FSH.Modules.Microfinance.Features.v1.FeeDefinitions.CreateFeeDefinition;

public class CreateFeeDefinitionValidator : AbstractValidator<CreateFeeDefinitionCommand>
{
    public CreateFeeDefinitionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
