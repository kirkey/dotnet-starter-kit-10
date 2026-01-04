namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.CreateLoanDisbursementTranche;

public class CreateLoanDisbursementTrancheValidator : AbstractValidator<CreateLoanDisbursementTrancheCommand>
{
    public CreateLoanDisbursementTrancheValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
