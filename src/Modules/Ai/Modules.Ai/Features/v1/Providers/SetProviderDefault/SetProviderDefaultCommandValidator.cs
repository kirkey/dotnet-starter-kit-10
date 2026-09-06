using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Providers;

namespace FSH.Modules.Ai.Features.v1.Providers.SetProviderDefault;

public sealed class SetProviderDefaultCommandValidator : AbstractValidator<SetProviderDefaultCommand>
{
    public SetProviderDefaultCommandValidator()
    {
        RuleFor(x => x.ProviderId).NotEmpty();
    }
}
