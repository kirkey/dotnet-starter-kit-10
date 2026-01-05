using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Brands;

public record GetBrandsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    string? OrderBy = null) : IQuery<BrandsPagedResponse>;

public record BrandsPagedResponse(
    List<BrandDto> Data,
    int TotalCount,
    int Page,
    int PageSize);
