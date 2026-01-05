using FluentValidation;
using FSH.Module.Catalog.Contracts.v1.Products;

namespace FSH.Module.Catalog.Features.v1.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).ValidateProductName();
        RuleFor(x => x.SKU).ValidateProductSKU();
        RuleFor(x => x.Description).ValidateProductDescription();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be non-negative");
        RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost must be non-negative").When(x => x.Cost.HasValue);
        RuleFor(x => x.QuantityInStock).GreaterThanOrEqualTo(0).WithMessage("Quantity must be non-negative");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand is required");
    }
}
