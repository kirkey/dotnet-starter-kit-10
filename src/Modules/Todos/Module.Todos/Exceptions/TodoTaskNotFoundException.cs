using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Todos.Exceptions;

/// <summary>
/// Exception thrown when a TodoTask entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, delete, or toggle a TodoTask
/// that doesn't exist in the database.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete task operations with invalid task ID
/// - Toggling completion status of non-existent task
/// - Reordering non-existent tasks
/// - Task doesn't belong to specified Todo
/// 
/// **Usage:**
/// throw new TodoTaskNotFoundException(taskId);
/// </summary>
public sealed class TodoTaskNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of TodoTaskNotFoundException.
    /// </summary>
    /// <param name="taskId">The ID of the task that was not found.</param>
    public TodoTaskNotFoundException(Guid taskId)
        : base($"Todo task with id '{taskId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of TodoTaskNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public TodoTaskNotFoundException(string message)
        : base(message) { }
}
