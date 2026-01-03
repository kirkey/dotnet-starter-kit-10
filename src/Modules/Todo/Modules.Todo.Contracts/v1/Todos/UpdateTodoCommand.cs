using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// Command to update an existing todo item.
/// 
/// **Purpose:**
/// Updates the details of a todo including name, description, notes, priority, and due date.
/// 
/// **Properties:**
/// - Id: The ID of the todo to update (required)
/// - Name: The updated name/title (required)
/// - Description: Updated description (optional)
/// - Notes: Updated notes (optional)
/// - Priority: Updated priority level (required, 1-4)
/// - DueDate: Updated due date (optional)
/// 
/// **Response:**
/// Returns the ID of the updated todo.
/// </summary>
public record UpdateTodoCommand : ICommand<Guid>
{
    /// <summary>
    /// Gets the ID of the todo to update (required).
    /// </summary>
    public required Guid Id { get; init; }
    
    /// <summary>
    /// Gets the updated name/title of the todo (required).
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the updated description (optional).
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets the updated notes (optional).
    /// </summary>
    public string? Notes { get; init; }
    
    /// <summary>
    /// Gets the updated priority level (1=Low, 2=Medium, 3=High, 4=Critical).
    /// </summary>
    public required int Priority { get; init; }
    
    /// <summary>
    /// Gets the updated due date (optional).
    /// </summary>
    public DateTimeOffset? DueDate { get; init; }
}
