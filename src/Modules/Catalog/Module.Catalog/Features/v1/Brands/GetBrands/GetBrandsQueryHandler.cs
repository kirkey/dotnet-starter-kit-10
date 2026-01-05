using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Brands;
using FSH.Module.Catalog.Data;

namespace FSH.Module.Catalog.Features.v1.Brands.GetBrands;

public sealed class GetBrandsQueryHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetBrandsQuery, BrandsPagedResponse>
{
    public async ValueTask<BrandsPagedResponse> Handle(
        GetBrandsQuery query,
        CancellationToken cancellationToken)
    {
        var brandQuery = context.Brands
            .AsNoTracking()
            .Where(b => b.TenantId == currentUser.GetTenant());

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim().ToLower();
            brandQuery = brandQuery.Where(b => b.Name.ToLower().Contains(searchTerm));
        }

        brandQuery = query.OrderBy switch
        {
            "name" => brandQuery.OrderBy(b => b.Name),
            "name_desc" => brandQuery.OrderByDescending(b => b.Name),
            _ => brandQuery.OrderBy(b => b.Name)
        };

        var brands = await brandQuery
            .Select(b => new BrandDto(
                b.Id,
                b.Name,
                b.Description,
                b.Website,
                b.IsActive,
                b.CreatedOnUtc,
                b.CreatedByUserName))
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await context.Brands
            .Where(b => b.TenantId == currentUser.GetTenant())
            .CountAsync(cancellationToken);

        return new BrandsPagedResponse(brands, totalCount, query.Page, query.PageSize);
    }
}
