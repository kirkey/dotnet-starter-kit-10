using FSH.Framework.Core.Paging;
using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Brands;

public record GetBrandsQuery : IQuery<PagedList<BrandDto>>
{
    public string? Search { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
    public string? OrderBy { get; init; }
}
