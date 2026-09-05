using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Brands;

namespace FSH.Modules.Catalog.Features.v1.Brands.RestoreBrand;

public sealed class RestoreBrandCommandValidator : AbstractValidator<RestoreBrandCommand>
{
    public RestoreBrandCommandValidator()
    {
        RuleFor(x => x.BrandId).NotEmpty();
    }
}
