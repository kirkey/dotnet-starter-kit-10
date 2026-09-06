using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Runs;

namespace FSH.Modules.Ai.Features.v1.Runs.StartAgentRun;

public sealed class StartAgentRunCommandValidator : AbstractValidator<StartAgentRunCommand>
{
    public StartAgentRunCommandValidator()
    {
        RuleFor(x => x.AgentId).NotEmpty();
        RuleFor(x => x.Input).NotEmpty().MaximumLength(8000);
    }
}
