namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.CreateLoanOfficerTarget;

public class CreateLoanOfficerTargetValidator : AbstractValidator<CreateLoanOfficerTargetCommand>
{
    public CreateLoanOfficerTargetValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
