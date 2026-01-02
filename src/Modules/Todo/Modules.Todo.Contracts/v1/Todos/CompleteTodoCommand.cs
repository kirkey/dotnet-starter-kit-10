using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record CompleteTodoCommand(Guid Id) : ICommand;
