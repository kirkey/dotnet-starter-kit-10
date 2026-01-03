namespace FSH.Modules.Microfinance.Features.v1.LoanApplications.CreateLoanApplication;

public class CreateLoanApplicationValidator : AbstractValidator<CreateLoanApplicationCommand>
{
    public CreateLoanApplicationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
