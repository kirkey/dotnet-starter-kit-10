using Mediator;

namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// Command to mark a todo as completed.
/// 
/// **Purpose:**
/// Marks an in-progress todo as completed and records the completion timestamp.
/// Sets the todo status to "Completed".
/// 
/// **Parameters:**
/// - Id: The ID of the todo to mark as completed
/// </summary>
public record CompleteTodoCommand(
    /// <summary>
    /// Gets the ID of the todo to mark as completed.
    /// </summary>
    Guid Id) : ICommand;
