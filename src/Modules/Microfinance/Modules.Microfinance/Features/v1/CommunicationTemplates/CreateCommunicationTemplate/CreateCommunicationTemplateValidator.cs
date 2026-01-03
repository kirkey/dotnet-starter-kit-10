namespace FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.CreateCommunicationTemplate;

public class CreateCommunicationTemplateValidator : AbstractValidator<CreateCommunicationTemplateCommand>
{
    public CreateCommunicationTemplateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
