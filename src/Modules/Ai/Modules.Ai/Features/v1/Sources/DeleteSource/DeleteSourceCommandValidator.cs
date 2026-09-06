using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Sources;

namespace FSH.Modules.Ai.Features.v1.Sources.DeleteSource;

public sealed class DeleteSourceCommandValidator : AbstractValidator<DeleteSourceCommand>
{
    public DeleteSourceCommandValidator()
    {
        RuleFor(x => x.SourceId).NotEmpty();
    }
}
