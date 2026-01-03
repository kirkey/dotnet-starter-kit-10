namespace FSH.Modules.Microfinance.Features.v1.TellerSessions.CreateTellerSession;

public class CreateTellerSessionValidator : AbstractValidator<CreateTellerSessionCommand>
{
    public CreateTellerSessionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
