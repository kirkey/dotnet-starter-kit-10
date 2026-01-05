using FSH.Module.Microfinance.Contracts.v1.TellerSessions.CreateTellerSession;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.CreateTellerSession;

public class CreateTellerSessionValidator : AbstractValidator<CreateTellerSessionCommand>
{
    public CreateTellerSessionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
