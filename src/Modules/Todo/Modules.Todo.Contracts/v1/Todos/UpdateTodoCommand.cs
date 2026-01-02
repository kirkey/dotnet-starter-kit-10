using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record UpdateTodoCommand(
    Guid Id,
    string Name,
    string? Description,
    string? Notes,
    int Priority,
    DateTimeOffset? DueDate) : ICommand<Guid>;
