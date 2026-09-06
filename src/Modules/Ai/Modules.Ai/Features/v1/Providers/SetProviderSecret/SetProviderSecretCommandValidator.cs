using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Providers;

namespace FSH.Modules.Ai.Features.v1.Providers.SetProviderSecret;

public sealed class SetProviderSecretCommandValidator : AbstractValidator<SetProviderSecretCommand>
{
    public SetProviderSecretCommandValidator()
    {
        RuleFor(x => x.ProviderId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty().MaximumLength(4096);
        RuleFor(x => x.KeyName).MaximumLength(128).When(x => x.KeyName is not null);
    }
}
