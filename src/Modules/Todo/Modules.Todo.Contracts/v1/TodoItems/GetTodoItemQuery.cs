using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Query to get a Todo Item by ID.
/// </summary>
public record GetTodoItemQuery(Guid Id) : IQuery<TodoItemResponse?>;

public record TodoItemResponse(
    Guid Id,
    Guid TodoListId,
    string Name,
    string? Description,
    string? Notes,
    string Status,
    bool IsActive,
    int Priority,
    int SortOrder,
    DateTimeOffset? DueDate,
    DateTimeOffset? CompletedDate,
    int? EstimatedHours,
    int? ActualHours,
    string? AssignedToUserId,
    string? AssignedToUserName,
    DateTimeOffset CreatedOnUtc,
    Guid? CreatedBy,
    string? CreatedByUserName,
    DateTimeOffset? LastModifiedOnUtc,
    Guid? LastModifiedBy,
    string? LastModifiedByUserName
);
