using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record DeleteTodoTaskCommand(Guid Id) : ICommand;
