using FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.CreateCollectionStrategy;

namespace FSH.Module.Microfinance.Features.v1.CollectionStrategys.CreateCollectionStrategy;

public class CreateCollectionStrategyValidator : AbstractValidator<CreateCollectionStrategyCommand>
{
    public CreateCollectionStrategyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
