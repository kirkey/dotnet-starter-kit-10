using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Categories;
using FSH.Module.Catalog.Data;

namespace FSH.Module.Catalog.Features.v1.Categories.GetCategories;

public sealed class GetCategoriesQueryHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetCategoriesQuery, CategoriesPagedResponse>
{
    public async ValueTask<CategoriesPagedResponse> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var categoryQuery = context.Categories
            .AsNoTracking()
            .Where(c => c.TenantId == currentUser.GetTenant());

        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var searchTerm = query.SearchTerm.Trim().ToLower();
            categoryQuery = categoryQuery.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.Code.ToLower().Contains(searchTerm));
        }

        if (query.ParentCategoryId.HasValue)
        {
            categoryQuery = categoryQuery.Where(c => c.ParentCategoryId == query.ParentCategoryId.Value);
        }

        categoryQuery = query.OrderBy switch
        {
            "name" => categoryQuery.OrderBy(c => c.Name),
            "name_desc" => categoryQuery.OrderByDescending(c => c.Name),
            "code" => categoryQuery.OrderBy(c => c.Code),
            "code_desc" => categoryQuery.OrderByDescending(c => c.Code),
            _ => categoryQuery.OrderBy(c => c.Name)
        };

        var categories = await categoryQuery
            .Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Code,
                c.Description,
                c.ParentCategoryId,
                c.ParentCategory != null ? c.ParentCategory.Name : null,
                c.IsActive,
                c.CreatedOnUtc,
                c.CreatedByUserName))
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await context.Categories
            .Where(c => c.TenantId == currentUser.GetTenant())
            .CountAsync(cancellationToken);

        return new CategoriesPagedResponse(categories, totalCount, query.Page, query.PageSize);
    }
}
