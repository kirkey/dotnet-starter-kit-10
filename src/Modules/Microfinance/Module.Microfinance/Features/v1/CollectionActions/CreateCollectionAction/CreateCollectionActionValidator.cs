namespace FSH.Module.Microfinance.Features.v1.CollectionActions.CreateCollectionAction;

public class CreateCollectionActionValidator : AbstractValidator<CreateCollectionActionCommand>
{
    public CreateCollectionActionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
