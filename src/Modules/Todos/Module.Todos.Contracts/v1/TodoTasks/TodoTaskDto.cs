namespace FSH.Module.Todos.Contracts.v1.TodoTasks;

/// <summary>
/// Data transfer object for a TodoTask item.
/// 
/// **Purpose:**
/// Contains all information about a single task within a todo.
/// Used when retrieving task details or listing tasks for a todo.
/// 
/// **Properties:**
/// All properties are immutable (record type).
/// </summary>
public record TodoTaskDto(
    /// <summary>
    /// Gets the unique identifier of the task.
    /// </summary>
    Guid Id,
    
    /// <summary>
    /// Gets the ID of the parent todo.
    /// </summary>
    Guid TodoId,
    
    /// <summary>
    /// Gets the name/title of the task.
    /// </summary>
    string Name,
    
    /// <summary>
    /// Gets the optional description of the task.
    /// </summary>
    string? Description,
    
    /// <summary>
    /// Gets the current status of the task (Pending, Completed).
    /// </summary>
    string Status,
    
    /// <summary>
    /// Gets a value indicating whether the task is completed.
    /// </summary>
    bool IsCompleted,
    
    /// <summary>
    /// Gets the timestamp when the task was completed (if IsCompleted is true).
    /// </summary>
    DateTimeOffset? CompletedAt,
    
    /// <summary>
    /// Gets the sort order position of the task within the parent todo.
    /// Lower values appear first.
    /// </summary>
    int SortOrder,
    
    /// <summary>
    /// Gets the timestamp when the task was created.
    /// </summary>
    DateTimeOffset CreatedOnUtc);
