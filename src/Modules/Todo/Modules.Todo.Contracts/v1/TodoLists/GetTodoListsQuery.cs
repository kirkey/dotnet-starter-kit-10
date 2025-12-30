using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoLists;

/// <summary>
/// Query to get all Todo Lists for the current tenant.
/// </summary>
public record GetTodoListsQuery(
    string? SearchTerm = null,
    string? Status = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 10
) : IQuery<TodoListsResponse>;

public record TodoListsResponse(
    List<TodoListSummaryDto> Data,
    int TotalCount,
    int Page,
    int PageSize
);

public record TodoListSummaryDto(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    bool IsActive,
    string? Color,
    int ItemCount,
    int CompletedItemCount,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName
);
