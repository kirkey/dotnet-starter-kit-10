using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Brands;
using FSH.Module.Catalog.Data;
using Mediator;

namespace FSH.Module.Catalog.Features.v1.Brands.GetBrands;

public sealed class GetBrandsQueryHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetBrandsQuery, List<BrandResponse>>
{
    public async ValueTask<List<BrandResponse>> Handle(
        GetBrandsQuery query,
        CancellationToken cancellationToken)
    {
        var brandQuery = context.Brands.AsNoTracking()
            .Where(b => b.TenantId == currentUser.GetTenant());
        
        if (!query.IncludeInactive)
        {
            brandQuery = brandQuery.Where(b => b.IsActive);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            brandQuery = brandQuery.Where(b => b.Name.ToLower().Contains(searchTerm));
        }
        
        var brands = await brandQuery
            .OrderBy(b => b.Name)
            .Select(b => new BrandResponse(
                b.Id,
                b.Name,
                b.Description,
                b.WebsiteUrl,
                b.IsActive,
                b.CreatedOnUtc,
                b.CreatedByUserName))
            .ToListAsync(cancellationToken);
        
        return brands;
    }
}
