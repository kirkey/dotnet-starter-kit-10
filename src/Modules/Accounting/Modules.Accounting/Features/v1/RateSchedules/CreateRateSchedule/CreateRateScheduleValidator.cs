using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.RateSchedules.CreateRateSchedule;

public class CreateRateScheduleValidator : AbstractValidator<CreateRateScheduleCommand>
{
    public CreateRateScheduleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
