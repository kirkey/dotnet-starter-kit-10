using Mediator;

namespace FSH.Modules.Todos.Contracts.v1.Todos;

/// <summary>
/// Command to update the status of a todo item.
/// 
/// **Purpose:**
/// Changes the status of a todo to one of: NotStarted, InProgress, Completed, or OnHold.
/// If status is set to Completed, the todo will be marked as completed automatically.
/// 
/// **Parameters:**
/// - Id: The ID of the todo to update
/// - Status: The new status code (0=NotStarted, 1=InProgress, 2=Completed, 3=OnHold)
/// </summary>
public record UpdateTodoStatusCommand(
    /// <summary>
    /// Gets the ID of the todo to update.
    /// </summary>
    Guid Id,
    
    /// <summary>
    /// Gets the new status code (0=NotStarted, 1=InProgress, 2=Completed, 3=OnHold).
    /// </summary>
    int Status) : ICommand;
