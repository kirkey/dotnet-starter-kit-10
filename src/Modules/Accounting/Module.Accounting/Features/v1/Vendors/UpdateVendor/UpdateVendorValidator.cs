using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Vendors.UpdateVendor;

namespace FSH.Module.Accounting.Features.v1.Vendors.UpdateVendor;

public class UpdateVendorValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
