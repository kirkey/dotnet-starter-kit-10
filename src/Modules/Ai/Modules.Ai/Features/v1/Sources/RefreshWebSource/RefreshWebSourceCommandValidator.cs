using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Sources;

namespace FSH.Modules.Ai.Features.v1.Sources.RefreshWebSource;

public sealed class RefreshWebSourceCommandValidator : AbstractValidator<RefreshWebSourceCommand>
{
    public RefreshWebSourceCommandValidator()
    {
        RuleFor(x => x.SourceId).NotEmpty();
    }
}
