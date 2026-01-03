namespace FSH.Modules.Microfinance.Features.v1.Loans.CreateLoan;

public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
