using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Command to update an existing Todo Item.
/// </summary>
public record UpdateTodoItemCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Notes = null,
    int? Priority = null,
    string? Status = null,
    DateTimeOffset? DueDate = null,
    int? EstimatedHours = null,
    int? ActualHours = null,
    bool? IsActive = null
) : ICommand<bool>;
