using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Products;

namespace FSH.Modules.Catalog.Features.v1.Products.RestoreProduct;

public sealed class RestoreProductCommandValidator : AbstractValidator<RestoreProductCommand>
{
    public RestoreProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
