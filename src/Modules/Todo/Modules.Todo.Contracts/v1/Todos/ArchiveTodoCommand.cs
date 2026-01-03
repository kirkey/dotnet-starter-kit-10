using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// Command to archive a todo item (soft delete).
/// 
/// **Purpose:**
/// Marks a todo as inactive/archived without permanent deletion.
/// Archived todos can be restored using the Restore functionality.
/// Use this instead of Delete to preserve historical data.
/// 
/// **Parameters:**
/// - Id: The ID of the todo to archive
/// </summary>
public record ArchiveTodoCommand(
    /// <summary>
    /// Gets the ID of the todo to archive.
    /// </summary>
    Guid Id) : ICommand;
