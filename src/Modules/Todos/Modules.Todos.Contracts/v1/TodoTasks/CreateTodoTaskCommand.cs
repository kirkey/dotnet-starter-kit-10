using Mediator;

namespace FSH.Modules.Todos.Contracts.v1.TodoTasks;

/// <summary>
/// Command to create a new task within a todo item.
/// 
/// **Purpose:**
/// Adds a new sub-task to an existing todo. Tasks allow breaking down
/// larger todos into smaller, manageable pieces of work.
/// 
/// **Properties:**
/// - TodoId: The ID of the parent todo (required)
/// - Name: The task title (required)
/// - Description: Optional task description
/// - SortOrder: Position within the todo's task list (default: 0)
/// 
/// **Response:**
/// Returns the GUID of the newly created task.
/// </summary>
public record CreateTodoTaskCommand : ICommand<Guid>
{
    /// <summary>
    /// Gets the ID of the parent todo (required).
    /// </summary>
    public required Guid TodoId { get; init; }
    
    /// <summary>
    /// Gets the name/title of the task (required).
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the optional description of the task.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets the sort order position of the task within the parent todo (default: 0).
    /// Lower values appear first in the list.
    /// </summary>
    public int SortOrder { get; init; }
}
