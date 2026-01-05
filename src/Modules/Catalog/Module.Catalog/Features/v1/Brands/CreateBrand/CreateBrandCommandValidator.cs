using FluentValidation;
using FSH.Module.Catalog.Contracts.v1.Brands;

namespace FSH.Module.Catalog.Features.v1.Brands.CreateBrand;

public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
{
    public CreateBrandCommandValidator()
    {
        RuleFor(x => x.Name).ValidateBrandName();
        RuleFor(x => x.Description).ValidateBrandDescription();
        RuleFor(x => x.WebsiteUrl).ValidateBrandWebsiteUrl();
    }
}
