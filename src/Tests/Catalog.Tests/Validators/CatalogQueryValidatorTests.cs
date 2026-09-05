using FSH.Modules.Catalog.Features.v1.Products.SearchProducts;
using FSH.Modules.Catalog.Features.v1.Products.ListTrashedProducts;
using FSH.Modules.Catalog.Features.v1.Categories.SearchCategories;
using FSH.Modules.Catalog.Features.v1.Categories.ListTrashedCategories;
using FSH.Modules.Catalog.Features.v1.Brands.SearchBrands;
using FSH.Modules.Catalog.Features.v1.Brands.ListTrashedBrands;
using FSH.Modules.Catalog.Contracts.v1.Products;
using FSH.Modules.Catalog.Contracts.v1.Categories;
using FSH.Modules.Catalog.Contracts.v1.Brands;
using Shouldly;
using Xunit;

namespace Catalog.Tests.Validators;

public sealed class CatalogQueryValidatorTests
{
    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 201)]
    public void SearchProducts_rejects_out_of_range_paging(int page, int size)
        => new SearchProductsQueryValidator().Validate(new SearchProductsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Fact]
    public void SearchProducts_accepts_valid_query()
        => new SearchProductsQueryValidator().Validate(new SearchProductsQuery()).IsValid.ShouldBeTrue();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 201)]
    public void ListTrashedProducts_rejects_out_of_range_paging(int page, int size)
        => new ListTrashedProductsQueryValidator().Validate(new ListTrashedProductsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 201)]
    public void SearchCategories_rejects_out_of_range_paging(int page, int size)
        => new SearchCategoriesQueryValidator().Validate(new SearchCategoriesQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 201)]
    public void ListTrashedCategories_rejects_out_of_range_paging(int page, int size)
        => new ListTrashedCategoriesQueryValidator().Validate(new ListTrashedCategoriesQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 201)]
    public void SearchBrands_rejects_out_of_range_paging(int page, int size)
        => new SearchBrandsQueryValidator().Validate(new SearchBrandsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 201)]
    public void ListTrashedBrands_rejects_out_of_range_paging(int page, int size)
        => new ListTrashedBrandsQueryValidator().Validate(new ListTrashedBrandsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Fact]
    public void ListTrashedBrands_accepts_valid_query()
        => new ListTrashedBrandsQueryValidator().Validate(new ListTrashedBrandsQuery()).IsValid.ShouldBeTrue();
}
