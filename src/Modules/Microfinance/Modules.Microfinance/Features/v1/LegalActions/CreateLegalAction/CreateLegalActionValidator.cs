namespace FSH.Modules.Microfinance.Features.v1.LegalActions.CreateLegalAction;

public class CreateLegalActionValidator : AbstractValidator<CreateLegalActionCommand>
{
    public CreateLegalActionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
