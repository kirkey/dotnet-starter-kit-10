namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.CreateLoanRepayment;

public class CreateLoanRepaymentValidator : AbstractValidator<CreateLoanRepaymentCommand>
{
    public CreateLoanRepaymentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
