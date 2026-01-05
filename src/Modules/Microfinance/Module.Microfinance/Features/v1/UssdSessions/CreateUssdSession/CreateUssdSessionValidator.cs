using FSH.Module.Microfinance.Contracts.v1.UssdSessions.CreateUssdSession;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.CreateUssdSession;

public class CreateUssdSessionValidator : AbstractValidator<CreateUssdSessionCommand>
{
    public CreateUssdSessionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
