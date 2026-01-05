using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.CreateCommunicationLog;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.CreateCommunicationLog;

public class CreateCommunicationLogValidator : AbstractValidator<CreateCommunicationLogCommand>
{
    public CreateCommunicationLogValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
