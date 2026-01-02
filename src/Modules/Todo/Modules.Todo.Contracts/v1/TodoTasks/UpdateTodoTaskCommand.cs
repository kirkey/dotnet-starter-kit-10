using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record UpdateTodoTaskCommand(
    Guid Id,
    string Name,
    string? Description,
    int SortOrder) : ICommand<Guid>;
