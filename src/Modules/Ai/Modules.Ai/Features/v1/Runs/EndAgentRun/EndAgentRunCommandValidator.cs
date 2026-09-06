using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Runs;

namespace FSH.Modules.Ai.Features.v1.Runs.EndAgentRun;

public sealed class EndAgentRunCommandValidator : AbstractValidator<EndAgentRunCommand>
{
    public EndAgentRunCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
