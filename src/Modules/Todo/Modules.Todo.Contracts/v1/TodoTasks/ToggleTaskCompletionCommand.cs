using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record ToggleTaskCompletionCommand(Guid Id) : ICommand;
