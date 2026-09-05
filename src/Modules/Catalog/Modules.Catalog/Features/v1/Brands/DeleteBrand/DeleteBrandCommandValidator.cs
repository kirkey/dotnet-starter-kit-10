using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Brands;

namespace FSH.Modules.Catalog.Features.v1.Brands.DeleteBrand;

public sealed class DeleteBrandCommandValidator : AbstractValidator<DeleteBrandCommand>
{
    public DeleteBrandCommandValidator()
    {
        RuleFor(x => x.BrandId).NotEmpty();
    }
}
