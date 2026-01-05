using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Products;
using FSH.Module.Catalog.Data;

namespace FSH.Module.Catalog.Features.v1.Products.GetProducts;

public sealed class GetProductsQueryHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetProductsQuery, ProductsPagedResponse>
{
    public async ValueTask<ProductsPagedResponse> Handle(
        GetProductsQuery query,
        CancellationToken cancellationToken)
    {
        var productQuery = context.Products
            .AsNoTracking()
            .Where(p => p.TenantId == currentUser.GetTenant());

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim().ToLower();
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

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            productQuery = productQuery.Where(p => p.Status == query.Status);
        }

        productQuery = query.OrderBy switch
        {
            "name" => productQuery.OrderBy(p => p.Name),
            "name_desc" => productQuery.OrderByDescending(p => p.Name),
            "price" => productQuery.OrderBy(p => p.Price),
            "price_desc" => productQuery.OrderByDescending(p => p.Price),
            _ => productQuery.OrderBy(p => p.Name)
        };

        var products = await productQuery
            .Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Description,
                p.SKU,
                p.Barcode,
                p.Price,
                p.Cost,
                p.QuantityInStock,
                p.ReorderLevel,
                p.Status,
                p.CategoryId,
                p.Category.Name,
                p.BrandId,
                p.Brand.Name,
                p.IsActive,
                p.CreatedOnUtc,
                p.CreatedByUserName))
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await context.Products
            .Where(p => p.TenantId == currentUser.GetTenant())
            .CountAsync(cancellationToken);

        return new ProductsPagedResponse(products, totalCount, query.Page, query.PageSize);
    }
}
