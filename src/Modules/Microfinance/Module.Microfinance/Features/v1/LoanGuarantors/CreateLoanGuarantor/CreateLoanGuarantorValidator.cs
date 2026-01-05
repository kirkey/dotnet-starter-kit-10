using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.CreateLoanGuarantor;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.CreateLoanGuarantor;

public class CreateLoanGuarantorValidator : AbstractValidator<CreateLoanGuarantorCommand>
{
    public CreateLoanGuarantorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
