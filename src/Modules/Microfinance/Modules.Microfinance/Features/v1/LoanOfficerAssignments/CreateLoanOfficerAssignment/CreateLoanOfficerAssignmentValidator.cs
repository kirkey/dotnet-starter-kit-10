namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerAssignments.CreateLoanOfficerAssignment;

public class CreateLoanOfficerAssignmentValidator : AbstractValidator<CreateLoanOfficerAssignmentCommand>
{
    public CreateLoanOfficerAssignmentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
