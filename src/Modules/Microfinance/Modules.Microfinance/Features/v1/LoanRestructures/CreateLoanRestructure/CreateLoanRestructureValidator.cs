namespace FSH.Modules.Microfinance.Features.v1.LoanRestructures.CreateLoanRestructure;

public class CreateLoanRestructureValidator : AbstractValidator<CreateLoanRestructureCommand>
{
    public CreateLoanRestructureValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
