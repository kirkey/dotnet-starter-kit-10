using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Agents;

namespace FSH.Modules.Ai.Features.v1.Agents.ArchiveAgent;

public sealed class ArchiveAgentCommandValidator : AbstractValidator<ArchiveAgentCommand>
{
    public ArchiveAgentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
