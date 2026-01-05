using FSH.Module.Microfinance.Contracts.v1.CollectionCases.CreateCollectionCase;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.CreateCollectionCase;

public class CreateCollectionCaseValidator : AbstractValidator<CreateCollectionCaseCommand>
{
    public CreateCollectionCaseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
