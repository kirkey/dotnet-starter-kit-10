using FluentValidation;
using FSH.Modules.Catalog.Contracts.v1.Products;

namespace FSH.Modules.Catalog.Features.v1.Products.DeleteProduct;

public sealed class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
    }
}
