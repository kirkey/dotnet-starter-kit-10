using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Products;

public record GetProductsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    Guid? CategoryId = null,
    Guid? BrandId = null,
    string? Status = null,
    string? OrderBy = null) : IQuery<ProductsPagedResponse>;

public record ProductsPagedResponse(
    List<ProductDto> Data,
    int TotalCount,
    int Page,
    int PageSize);
