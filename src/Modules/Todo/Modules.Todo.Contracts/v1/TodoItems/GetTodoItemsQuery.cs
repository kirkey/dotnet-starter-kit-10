using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoItems;

/// <summary>
/// Query to get all Todo Items for a specific Todo List.
/// </summary>
public record GetTodoItemsQuery(Guid TodoListId) : IQuery<List<TodoItemResponse>>;
