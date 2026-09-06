using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Agents;

namespace FSH.Modules.Ai.Features.v1.Agents.CreateAgentCopy;

public sealed class CreateAgentCopyCommandValidator : AbstractValidator<CreateAgentCopyCommand>
{
    public CreateAgentCopyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
