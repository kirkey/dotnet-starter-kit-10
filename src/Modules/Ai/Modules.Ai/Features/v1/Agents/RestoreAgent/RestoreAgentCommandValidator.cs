using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Agents;

namespace FSH.Modules.Ai.Features.v1.Agents.RestoreAgent;

public sealed class RestoreAgentCommandValidator : AbstractValidator<RestoreAgentCommand>
{
    public RestoreAgentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
