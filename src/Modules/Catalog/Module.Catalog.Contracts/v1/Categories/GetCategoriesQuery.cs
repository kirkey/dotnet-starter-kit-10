using Mediator;

namespace FSH.Module.Catalog.Contracts.v1.Categories;

public record GetCategoriesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    Guid? ParentCategoryId = null,
    string? OrderBy = null) : IQuery<CategoriesPagedResponse>;

public record CategoriesPagedResponse(
    List<CategoryDto> Data,
    int TotalCount,
    int Page,
    int PageSize);
