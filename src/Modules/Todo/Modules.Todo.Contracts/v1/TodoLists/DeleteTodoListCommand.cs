using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoLists;

/// <summary>
/// Command to delete a Todo List.
/// </summary>
public record DeleteTodoListCommand(Guid Id) : ICommand<Unit>;
