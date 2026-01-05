using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Products;
using FSH.Module.Catalog.Data;
using Mediator;

namespace FSH.Module.Catalog.Features.v1.Products.GetProducts;

public sealed class GetProductsQueryHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetProductsQuery, List<ProductResponse>>
{
    public async ValueTask<List<ProductResponse>> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var productQuery = context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .Where(p => p.TenantId == currentUser.GetTenant());
        
        if (!query.IncludeInactive)
        {
            productQuery = productQuery.Where(p => p.IsActive);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            productQuery = productQuery.Where(p =>
                p.Name.ToLower().Contains(searchTerm) ||
                p.SKU.ToLower().Contains(searchTerm));
        }
        
        if (query.CategoryId.HasValue)
        {
            productQuery = productQuery.Where(p => p.CategoryId == query.CategoryId.Value);
        }
        
        if (query.BrandId.HasValue)
        {
            productQuery = productQuery.Where(p => p.BrandId == query.BrandId.Value);
        }
        
        var products = await productQuery
            .OrderBy(p => p.Name)
            .Select(p => new ProductResponse(
                p.Id,
                p.Name,
                p.SKU,
                p.Description,
                p.Barcode,
                p.Price,
                p.Cost,
                p.QuantityInStock,
                p.Specifications,
                p.CategoryId,
                p.Category.Name,
                p.BrandId,
                p.Brand.Name,
                p.IsActive,
                p.CreatedOnUtc,
                p.CreatedByUserName))
            .ToListAsync(cancellationToken);
        
        return products;
    }
}
