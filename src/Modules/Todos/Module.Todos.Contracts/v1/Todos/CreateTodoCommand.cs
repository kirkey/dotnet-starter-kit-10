using Mediator;

namespace FSH.Module.Todos.Contracts.v1.Todos;

/// <summary>
/// Command to create a new todo item.
/// 
/// **Purpose:**
/// Initiates the creation of a new todo with the provided details.
/// 
/// **Properties:**
/// - Name: The title/name of the todo (required)
/// - Description: Optional detailed description
/// - Notes: Optional additional notes
/// - Priority: Priority level (1-4, mapped to TodoPriority enum)
/// - DueDate: Optional target completion date
/// 
/// **Response:**
/// Returns the GUID of the newly created todo item.
/// </summary>
public record CreateTodoCommand : ICommand<Guid>
{
    /// <summary>
    /// Gets the name/title of the todo to create (required).
    /// </summary>
    public required string Name { get; init; }
    
    /// <summary>
    /// Gets the description of the todo (optional).
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets additional notes for the todo (optional).
    /// </summary>
    public string? Notes { get; init; }
    
    /// <summary>
    /// Gets the priority level (1=Low, 2=Medium, 3=High, 4=Critical).
    /// </summary>
    public required int Priority { get; init; }
    
    /// <summary>
    /// Gets the optional due date for the todo.
    /// </summary>
    public DateTimeOffset? DueDate { get; init; }
}
