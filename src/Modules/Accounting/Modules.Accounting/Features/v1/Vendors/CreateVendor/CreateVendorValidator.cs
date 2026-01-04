using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Vendors.CreateVendor;

public class CreateVendorValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorValidator()
    {
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
