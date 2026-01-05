using FSH.Framework.Core.Paging;
using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Products;

public record GetProductsQuery : IQuery<PagedList<ProductDto>>
{
    public string? Search { get; init; }
    public Guid? CategoryId { get; init; }
    public Guid? BrandId { get; init; }
    public string? Status { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? OrderBy { get; init; }
}
