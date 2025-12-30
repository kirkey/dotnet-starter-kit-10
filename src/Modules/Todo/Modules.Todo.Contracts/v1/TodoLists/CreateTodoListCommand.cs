using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoLists;

/// <summary>
/// Command to create a new Todo List.
/// </summary>
public record CreateTodoListCommand(
    string Name,
    string? Description = null,
    string? Notes = null,
    string? Color = null
) : ICommand<Guid>;
