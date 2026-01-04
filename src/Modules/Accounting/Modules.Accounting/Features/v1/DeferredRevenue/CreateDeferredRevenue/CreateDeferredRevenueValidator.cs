using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.DeferredRevenue.CreateDeferredRevenue;

public class CreateDeferredRevenueValidator : AbstractValidator<CreateDeferredRevenueCommand>
{
    public CreateDeferredRevenueValidator()
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
