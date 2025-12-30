using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Command to complete a Todo Item.
/// </summary>
public record CompleteTodoItemCommand(Guid Id) : ICommand<bool>;
