using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.TodoTasks;

/// <summary>
/// Command to update an existing task within a todo item.
/// 
/// **Purpose:**
/// Updates the details of a task including name, description, and sort order.
/// 
/// **Properties:**
/// - Id: The ID of the task to update (required)
/// - Name: The updated task title (required)
/// - Description: Updated task description (optional)
/// - SortOrder: Updated position within the todo's task list (required)
/// 
/// **Response:**
/// Returns the ID of the updated task.
/// </summary>
public record UpdateTodoTaskCommand : ICommand<Guid>
{
    /// <summary>
    /// Gets the ID of the task to update (required).
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Gets the updated name/title of the task (required).
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the updated description (optional).
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets the updated sort order position within the parent todo (required).
    /// </summary>
    public required int SortOrder { get; init; }
}
