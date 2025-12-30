using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoLists;

/// <summary>
/// Query to get a Todo List by ID with its items.
/// </summary>
public record GetTodoListQuery(Guid Id) : IQuery<TodoListResponse?>;

public record TodoListResponse(
    Guid Id,
    string Name,
    string? Description,
    string? Notes,
    string Status,
    bool IsActive,
    string? Color,
    int SortOrder,
    DateTimeOffset? DueDate,
    DateTimeOffset CreatedOnUtc,
    Guid? CreatedBy,
    string? CreatedByUserName,
    DateTimeOffset? LastModifiedOnUtc,
    Guid? LastModifiedBy,
    string? LastModifiedByUserName,
    List<TodoItemDto> Items
);

public record TodoItemDto(
    Guid Id,
    string Name,
    string? Description,
    string Status,
    int Priority,
    DateTimeOffset? DueDate,
    DateTimeOffset? CompletedDate,
    string? AssignedToUserName
);
