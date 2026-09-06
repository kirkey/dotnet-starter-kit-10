using FluentValidation;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Schedules;

namespace FSH.Modules.Ai.Features.v1.Schedules.UpdateSchedule;

public sealed class UpdateScheduleCommandValidator : AbstractValidator<UpdateScheduleCommand>
{
    public UpdateScheduleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cron)
            .NotEmpty()
            .MaximumLength(100)
            .Must(ScheduleSupport.HasCronShape)
            .WithMessage("Cron must have five parts (minute hour day month weekday).");
        RuleFor(x => x.Url)
            .Must(url => Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .When(x => x.TaskType == ScheduleTaskType.WebWatch)
            .WithMessage("Web-watch schedules require an absolute http(s) URL.")
            .MaximumLength(2048);
        RuleFor(x => x.Prompt)
            .NotEmpty()
            .MaximumLength(8000)
            .When(x => x.TaskType == ScheduleTaskType.Prompt);
        RuleFor(x => x.Recipients.Count).LessThanOrEqualTo(20);
        RuleForEach(x => x.Recipients).EmailAddress().MaximumLength(256);
    }
}
