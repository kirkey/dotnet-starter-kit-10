using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Vendors.CreateVendor;
using FSH.Module.Accounting.Features;

namespace FSH.Module.Accounting.Features.v1.Vendors.CreateVendor;

public class CreateVendorValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.Name)
            .ValidateVendorName();
            
        RuleFor(x => x.Description)
            .ValidateDescription();
    }
}
