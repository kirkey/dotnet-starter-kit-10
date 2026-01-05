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
        RuleFor(x => x.Barcode).ValidateProductBarcode();
        RuleFor(x => x.Price).ValidateProductPrice();
        RuleFor(x => x.Cost).ValidateProductCost();
        RuleFor(x => x.QuantityInStock).ValidateProductQuantity();
        RuleFor(x => x.Specifications).ValidateProductSpecifications();
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.BrandId).NotEmpty().WithMessage("Brand is required");
    }
}
