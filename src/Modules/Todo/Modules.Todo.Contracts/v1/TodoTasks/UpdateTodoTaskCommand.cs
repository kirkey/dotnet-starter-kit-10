using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record UpdateTodoTaskCommand : ICommand<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required int SortOrder { get; init; }
}
