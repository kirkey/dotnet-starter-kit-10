using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Schedules;

namespace FSH.Modules.Ai.Features.v1.Schedules.DeleteSchedule;

public sealed class DeleteScheduleCommandValidator : AbstractValidator<DeleteScheduleCommand>
{
    public DeleteScheduleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
