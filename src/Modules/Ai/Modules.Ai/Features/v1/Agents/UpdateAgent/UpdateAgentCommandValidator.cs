using FluentValidation;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Agents;

namespace FSH.Modules.Ai.Features.v1.Agents.UpdateAgent;

public sealed class UpdateAgentCommandValidator : AbstractValidator<UpdateAgentCommand>
{
    public UpdateAgentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Instructions).NotEmpty().MaximumLength(20000);
        RuleFor(x => x.RuntimeBinding).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(256);
        RuleFor(x => x.Variant).IsInEnum();
        RuleFor(x => x.Skills.Count).LessThanOrEqualTo(50);
        RuleForEach(x => x.Skills).NotEmpty().MaximumLength(100);
        RuleFor(x => x.AccessUserIds)
            .Must(ids => ids.Count > 0)
            .When(x => x.AccessMode == AgentAccessMode.Selected)
            .WithMessage("Selected access mode requires at least one user.");
        RuleForEach(x => x.AccessUserIds).NotEmpty().MaximumLength(64);
    }
}
