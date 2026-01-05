using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.CreateCommunicationTemplate;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.CreateCommunicationTemplate;

public class CreateCommunicationTemplateValidator : AbstractValidator<CreateCommunicationTemplateCommand>
{
    public CreateCommunicationTemplateValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
