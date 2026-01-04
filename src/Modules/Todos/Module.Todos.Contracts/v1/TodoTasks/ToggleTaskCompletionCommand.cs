using Mediator;

namespace FSH.Module.Todos.Contracts.v1.TodoTasks;

/// <summary>
/// Command to toggle the completion status of a task.
/// 
/// **Purpose:**
/// Flips the completion status of a task. If completed, marks as pending.
/// If pending, marks as completed with a timestamp.
/// Provides a convenient way to quickly update task status.
/// 
/// **Parameters:**
/// - Id: The ID of the task to toggle
/// </summary>
public record ToggleTaskCompletionCommand(
    /// <summary>
    /// Gets the ID of the task to toggle.
    /// </summary>
    Guid Id) : ICommand;
