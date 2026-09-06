using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Providers;

namespace FSH.Modules.Ai.Features.v1.Providers.DeleteProvider;

public sealed class DeleteProviderCommandValidator : AbstractValidator<DeleteProviderCommand>
{
    public DeleteProviderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
