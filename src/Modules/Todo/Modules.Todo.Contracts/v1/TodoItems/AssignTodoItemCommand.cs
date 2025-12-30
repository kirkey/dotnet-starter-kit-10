using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Command to assign a Todo Item to a user.
/// </summary>
public record AssignTodoItemCommand(
    Guid Id,
    string UserId,
    string UserName
) : ICommand<bool>;
