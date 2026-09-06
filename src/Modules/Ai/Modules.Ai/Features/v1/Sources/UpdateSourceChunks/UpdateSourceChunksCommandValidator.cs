using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Sources;

namespace FSH.Modules.Ai.Features.v1.Sources.UpdateSourceChunks;

public sealed class UpdateSourceChunksCommandValidator : AbstractValidator<UpdateSourceChunksCommand>
{
    public UpdateSourceChunksCommandValidator()
    {
        RuleFor(x => x.SourceId).NotEmpty();
    }
}
