using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoLists;

/// <summary>
/// Command to update an existing Todo List.
/// </summary>
public record UpdateTodoListCommand(
    Guid Id,
    string Name,
    string? Description = null,
    string? Notes = null,
    string? Color = null,
    string? Status = null,
    bool? IsActive = null
) : ICommand<bool>;
