namespace FSH.Module.Microfinance.Features.v1.CustomerCases.CreateCustomerCase;

public class CreateCustomerCaseValidator : AbstractValidator<CreateCustomerCaseCommand>
{
    public CreateCustomerCaseValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
