using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record CreateTodoTaskCommand(
    Guid TodoId,
    string Name,
    string? Description,
    int SortOrder = 0) : ICommand<Guid>;
