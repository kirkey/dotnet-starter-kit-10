using Mediator;

namespace FSH.Module.Todos.Contracts.v1.Todos;

/// <summary>
/// Command to reopen a completed todo.
/// 
/// **Purpose:**
/// Marks a completed todo as not completed and changes its status back to "InProgress".
/// Clears the completion timestamp.
/// Use this when a completed todo needs to be worked on again.
/// 
/// **Parameters:**
/// - Id: The ID of the todo to reopen
/// </summary>
public record ReopenTodoCommand(
    /// <summary>
    /// Gets the ID of the todo to reopen.
    /// </summary>
    Guid Id) : ICommand;
