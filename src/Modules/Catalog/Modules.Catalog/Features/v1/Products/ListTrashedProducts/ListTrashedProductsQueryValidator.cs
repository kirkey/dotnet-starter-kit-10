using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Products;

namespace FSH.Modules.Catalog.Features.v1.Products.ListTrashedProducts;

public sealed class ListTrashedProductsQueryValidator : AbstractValidator<ListTrashedProductsQuery>
{
    public ListTrashedProductsQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 200)
            .WithMessage("Page size must be between 1 and 200.");
    }
}
