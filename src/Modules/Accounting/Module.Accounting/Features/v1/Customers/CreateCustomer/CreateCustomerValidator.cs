using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Customers.CreateCustomer;
using FSH.Module.Accounting.Features;

namespace FSH.Module.Accounting.Features.v1.Customers.CreateCustomer;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.Name)
            .ValidateCustomerName();
            
        RuleFor(x => x.Description)
            .ValidateDescription();
    }
}
