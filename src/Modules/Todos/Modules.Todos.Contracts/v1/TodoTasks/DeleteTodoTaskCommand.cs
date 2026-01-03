using Mediator;

namespace FSH.Modules.Todos.Contracts.v1.TodoTasks;

/// <summary>
/// Command to delete a task from a todo item.
/// 
/// **Purpose:**
/// Permanently removes a task from its parent todo.
/// 
/// **Parameters:**
/// - Id: The ID of the task to delete
/// </summary>
public record DeleteTodoTaskCommand(
    /// <summary>
    /// Gets the ID of the task to delete.
    /// </summary>
    Guid Id) : ICommand;
