using Mediator;

namespace FSH.Modules.Todos.Contracts.v1.TodoTasks;

/// <summary>
/// Command to reorder tasks within a todo item.
/// 
/// **Purpose:**
/// Updates the sort order of multiple tasks in a single operation.
/// Useful for drag-and-drop reordering functionality.
/// 
/// **Parameters:**
/// - TodoId: The ID of the parent todo
/// - Tasks: List of task IDs with their new sort order positions
/// </summary>
public record ReorderTasksCommand(
    /// <summary>
    /// Gets the ID of the parent todo.
    /// </summary>
    Guid TodoId,
    
    /// <summary>
    /// Gets the list of tasks with their new sort order positions.
    /// </summary>
    List<TaskOrderItem> Tasks) : ICommand;

/// <summary>
/// Represents a task and its new sort order position.
/// 
/// **Purpose:**
/// Used in reordering operations to specify which task should have which position.
/// 
/// **Properties:**
/// - TaskId: The ID of the task
/// - SortOrder: The new position in the task list (lower values appear first)
/// </summary>
public record TaskOrderItem(
    /// <summary>
    /// Gets the ID of the task being reordered.
    /// </summary>
    Guid TaskId,
    
    /// <summary>
    /// Gets the new sort order position for this task.
    /// </summary>
    int SortOrder);
