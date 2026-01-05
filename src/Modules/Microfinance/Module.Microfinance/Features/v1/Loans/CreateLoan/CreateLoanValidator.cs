using FSH.Module.Microfinance.Contracts.v1.Loans;

namespace FSH.Module.Microfinance.Features.v1.Loans.CreateLoan;

public class CreateLoanValidator : AbstractValidator<CreateLoanCommand>
{
    public CreateLoanValidator()
    {
        RuleFor(x => x.Name).ValidateName();
    }
}
