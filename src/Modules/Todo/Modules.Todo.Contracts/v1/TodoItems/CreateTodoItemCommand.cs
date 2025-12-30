using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Command to create a new Todo Item.
/// </summary>
public record CreateTodoItemCommand(
    Guid TodoListId,
    string Name,
    string? Description = null,
    string? Notes = null,
    int Priority = 2,
    DateTimeOffset? DueDate = null,
    int? EstimatedHours = null
) : ICommand<Guid>;
