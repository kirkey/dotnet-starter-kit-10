using FSH.Framework.Core.Exceptions;

namespace FSH.Modules.Todos.Exceptions;

/// <summary>
/// Exception thrown when a parent Todo is not found for a TodoTask operation.
/// 
/// **Purpose:**
/// Raised when attempting to create or operate on a task for a Todo that doesn't exist.
/// This maintains data integrity by ensuring tasks only belong to existing todos.
/// 
/// **HTTP Mapping:**
/// Returns 400 Bad Request status code
/// 
/// **When Used:**
/// - Creating a task for a non-existent parent Todo
/// - The referenced parent Todo ID is invalid
/// - Parent Todo belongs to different tenant
/// 
/// **Usage:**
/// throw new ParentTodoNotFoundException(todoId);
/// </summary>
public sealed class ParentTodoNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of ParentTodoNotFoundException.
    /// </summary>
    /// <param name="todoId">The ID of the parent Todo that was not found.</param>
    public ParentTodoNotFoundException(Guid todoId)
        : base($"Parent todo with id '{todoId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of ParentTodoNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public ParentTodoNotFoundException(string message)
        : base(message) { }
}
