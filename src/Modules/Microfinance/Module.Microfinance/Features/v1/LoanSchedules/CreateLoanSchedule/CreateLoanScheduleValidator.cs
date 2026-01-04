namespace FSH.Module.Microfinance.Features.v1.LoanSchedules.CreateLoanSchedule;

public class CreateLoanScheduleValidator : AbstractValidator<CreateLoanScheduleCommand>
{
    public CreateLoanScheduleValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
