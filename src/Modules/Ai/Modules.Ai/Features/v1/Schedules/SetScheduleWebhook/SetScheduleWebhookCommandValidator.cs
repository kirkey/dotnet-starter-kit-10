using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Schedules;

namespace FSH.Modules.Ai.Features.v1.Schedules.SetScheduleWebhook;

public sealed class SetScheduleWebhookCommandValidator : AbstractValidator<SetScheduleWebhookCommand>
{
    public SetScheduleWebhookCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
