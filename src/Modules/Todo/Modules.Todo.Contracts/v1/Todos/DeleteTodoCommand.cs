using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record DeleteTodoCommand(Guid Id) : ICommand;
