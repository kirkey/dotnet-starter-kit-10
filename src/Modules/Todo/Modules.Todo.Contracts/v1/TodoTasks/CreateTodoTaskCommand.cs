using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record CreateTodoTaskCommand : ICommand<Guid>
{
    public required Guid TodoId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int SortOrder { get; init; }
}
