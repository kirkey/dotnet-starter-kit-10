using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Todos.Exceptions;

/// <summary>
/// Exception thrown when a Todo entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete a Todo that doesn't exist
/// in the database or has been archived.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid Todo ID
/// - Operating on archived todos (depending on context)
/// - Todo referenced doesn't belong to current tenant
/// 
/// **Usage:**
/// throw new TodoNotFoundException(todoId);
/// </summary>
public sealed class TodoNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of TodoNotFoundException.
    /// </summary>
    /// <param name="todoId">The ID of the todo that was not found.</param>
    public TodoNotFoundException(Guid todoId)
        : base($"Todo with id '{todoId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of TodoNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public TodoNotFoundException(string message)
        : base(message) { }
}
