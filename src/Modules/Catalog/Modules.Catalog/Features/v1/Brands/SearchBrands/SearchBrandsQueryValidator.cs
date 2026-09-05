using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Brands;

namespace FSH.Modules.Catalog.Features.v1.Brands.SearchBrands;

public sealed class SearchBrandsQueryValidator : AbstractValidator<SearchBrandsQuery>
{
    public SearchBrandsQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 200)
            .WithMessage("Page size must be between 1 and 200.");
    }
}
