using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Command to delete a Todo Item.
/// </summary>
public record DeleteTodoItemCommand(Guid Id) : ICommand;
