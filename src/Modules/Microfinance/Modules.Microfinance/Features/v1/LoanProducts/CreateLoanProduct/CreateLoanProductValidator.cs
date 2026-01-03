namespace FSH.Modules.Microfinance.Features.v1.LoanProducts.CreateLoanProduct;

public class CreateLoanProductValidator : AbstractValidator<CreateLoanProductCommand>
{
    public CreateLoanProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
