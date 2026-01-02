using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

public record GetTodoTasksQuery(Guid TodoId) : IQuery<List<TodoTaskDto>>;
