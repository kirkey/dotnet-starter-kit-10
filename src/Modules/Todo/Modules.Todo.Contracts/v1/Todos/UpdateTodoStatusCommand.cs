using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record UpdateTodoStatusCommand(Guid Id, int Status) : ICommand;
