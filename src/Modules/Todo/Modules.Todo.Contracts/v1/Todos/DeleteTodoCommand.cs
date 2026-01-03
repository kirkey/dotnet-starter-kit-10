using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// Command to delete a todo item permanently.
/// 
/// **Purpose:**
/// Permanently removes a todo and all its associated tasks from the database.
/// 
/// **Warning:**
/// This is a hard delete operation and cannot be undone. Use Archive for soft deletion instead.
/// 
/// **Parameters:**
/// - Id: The ID of the todo to delete
/// </summary>
public record DeleteTodoCommand(
    /// <summary>
    /// Gets the ID of the todo to delete.
    /// </summary>
    Guid Id) : ICommand;
