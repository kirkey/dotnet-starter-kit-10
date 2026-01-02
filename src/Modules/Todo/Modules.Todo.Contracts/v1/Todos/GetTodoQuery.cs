using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

public record GetTodoQuery(Guid Id) : IQuery<TodoDto>;
