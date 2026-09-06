using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Schedules;

namespace FSH.Modules.Ai.Features.v1.Schedules.StartScheduledRun;

public sealed class StartScheduledRunCommandValidator : AbstractValidator<StartScheduledRunCommand>
{
    public StartScheduledRunCommandValidator()
    {
        RuleFor(x => x.ScheduleId).NotEmpty();
        RuleFor(x => x.Token).NotEmpty().MaximumLength(64);
    }
}
