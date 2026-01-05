using FSH.Framework.Core.Context;
using FSH.Module.Catalog.Contracts.v1.Categories;
using FSH.Module.Catalog.Data;
using Mediator;

namespace FSH.Module.Catalog.Features.v1.Categories.GetCategories;

/// <summary>
/// Handles retrieving list of categories with filtering.
/// </summary>
public sealed class GetCategoriesQueryHandler(
    CatalogDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    public async ValueTask<List<CategoryResponse>> Handle(
        GetCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        // Start query - read-only, no tracking
        var categoryQuery = context.Categories.AsNoTracking()
            .Where(c => c.TenantId == currentUser.GetTenant());
        
        // Apply active filter
        if (!query.IncludeInactive)
        {
            categoryQuery = categoryQuery.Where(c => c.IsActive);
        }
        
        // Apply search filter
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            categoryQuery = categoryQuery.Where(c =>
                c.Name.ToLower().Contains(searchTerm) ||
                c.Code.ToLower().Contains(searchTerm));
        }
        
        // Apply parent filter
        if (query.ParentId.HasValue)
        {
            categoryQuery = categoryQuery.Where(c => c.ParentId == query.ParentId.Value);
        }
        
        // Project to DTO
        var categories = await categoryQuery
            .OrderBy(c => c.Name)
            .Select(c => new CategoryResponse(
                c.Id,
                c.Name,
                c.Code,
                c.Description,
                c.ParentId,
                c.Parent != null ? c.Parent.Name : null,
                c.IsActive,
                c.CreatedOnUtc,
                c.CreatedByUserName))
            .ToListAsync(cancellationToken);
        
        return categories;
    }
}
