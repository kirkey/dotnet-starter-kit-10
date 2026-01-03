namespace FSH.Modules.Microfinance.Features.v1.LoanGuarantors.CreateLoanGuarantor;

public class CreateLoanGuarantorValidator : AbstractValidator<CreateLoanGuarantorCommand>
{
    public CreateLoanGuarantorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
