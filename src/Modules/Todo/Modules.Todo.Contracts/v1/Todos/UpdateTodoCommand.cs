using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record UpdateTodoCommand : ICommand<Guid>
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public string? Notes { get; init; }
    public required int Priority { get; init; }
    public DateTimeOffset? DueDate { get; init; }
}
