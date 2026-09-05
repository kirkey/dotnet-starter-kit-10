using FSH.Modules.Catalog.Features.v1.Products.DeleteProduct;
using FSH.Modules.Catalog.Features.v1.Products.RestoreProduct;
using FSH.Modules.Catalog.Features.v1.Categories.DeleteCategory;
using FSH.Modules.Catalog.Features.v1.Categories.RestoreCategory;
using FSH.Modules.Catalog.Features.v1.Brands.DeleteBrand;
using FSH.Modules.Catalog.Features.v1.Brands.RestoreBrand;
using FSH.Modules.Catalog.Contracts.v1.Products;
using FSH.Modules.Catalog.Contracts.v1.Categories;
using FSH.Modules.Catalog.Contracts.v1.Brands;
using Shouldly;
using Xunit;

namespace Catalog.Tests.Validators;

public sealed class CatalogCommandValidatorTests
{
    [Fact]
    public void DeleteProduct_rejects_empty_id()
        => new DeleteProductCommandValidator().Validate(new DeleteProductCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void RestoreProduct_rejects_empty_id()
        => new RestoreProductCommandValidator().Validate(new RestoreProductCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void DeleteCategory_rejects_empty_id()
        => new DeleteCategoryCommandValidator().Validate(new DeleteCategoryCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void RestoreCategory_rejects_empty_id()
        => new RestoreCategoryCommandValidator().Validate(new RestoreCategoryCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void DeleteBrand_rejects_empty_id()
        => new DeleteBrandCommandValidator().Validate(new DeleteBrandCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void RestoreBrand_rejects_empty_id()
        => new RestoreBrandCommandValidator().Validate(new RestoreBrandCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void Accepts_valid_ids()
    {
        new DeleteProductCommandValidator().Validate(new DeleteProductCommand(Guid.NewGuid())).IsValid.ShouldBeTrue();
        new RestoreCategoryCommandValidator().Validate(new RestoreCategoryCommand(Guid.NewGuid())).IsValid.ShouldBeTrue();
        new DeleteBrandCommandValidator().Validate(new DeleteBrandCommand(Guid.NewGuid())).IsValid.ShouldBeTrue();
    }
}
