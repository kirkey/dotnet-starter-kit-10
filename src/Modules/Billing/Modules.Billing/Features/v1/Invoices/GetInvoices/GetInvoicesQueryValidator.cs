using FluentValidation;
using FSH.Modules.Billing.Contracts.v1.Invoices;

namespace FSH.Modules.Billing.Features.v1.Invoices.GetInvoices;

public sealed class GetInvoicesQueryValidator : AbstractValidator<GetInvoicesQuery>
{
    public GetInvoicesQueryValidator()
    {
        // NOTE: this query intentionally does not implement IPagedQuery (non-nullable
        // paging with endpoint-side clamping), so the shared PagedQueryValidator<T>
        // cannot apply — these inline rules mirror its bounds.
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(q => q.PeriodMonth)
            .InclusiveBetween(1, 12)
            .When(q => q.PeriodMonth.HasValue);
    }
}
