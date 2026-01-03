using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

/// <summary>
/// Query to retrieve all tasks for a specific todo item.
/// 
/// **Purpose:**
/// Fetches all tasks associated with a todo, ordered by their sort order.
/// 
/// **Parameters:**
/// - TodoId: The ID of the todo to get tasks for
/// 
/// **Response:**
/// Returns a list of TodoTaskDto objects ordered by sort order.
/// </summary>
public record GetTodoTasksQuery(
    /// <summary>
    /// Gets the ID of the parent todo.
    /// </summary>
    Guid TodoId) : IQuery<List<TodoTaskDto>>;
