using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

public class CreateLoanDisbursementTrancheValidator : AbstractValidator<CreateLoanDisbursementTrancheCommand>
{
    public CreateLoanDisbursementTrancheValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
