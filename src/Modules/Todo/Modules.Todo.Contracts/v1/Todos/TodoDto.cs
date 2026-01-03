namespace FSH.Modules.Todo.Contracts.v1.Todos;

/// <summary>
/// Complete data transfer object for a Todo item.
/// 
/// **Purpose:**
/// Contains all detailed information about a single todo item including
/// task counts, completion information, and audit trail.
/// Used in GetTodo queries and when returning full todo details.
/// 
/// **Properties:**
/// All properties are immutable (record with init-only properties).
/// </summary>
public record TodoDto(
    /// <summary>
    /// Gets the unique identifier of the todo.
    /// </summary>
    Guid Id,
    
    /// <summary>
    /// Gets the name/title of the todo.
    /// </summary>
    string Name,
    
    /// <summary>
    /// Gets the optional description providing more details.
    /// </summary>
    string? Description,
    
    /// <summary>
    /// Gets optional notes associated with the todo.
    /// </summary>
    string? Notes,
    
    /// <summary>
    /// Gets the current status of the todo (NotStarted, InProgress, Completed, OnHold).
    /// </summary>
    string Status,
    
    /// <summary>
    /// Gets a value indicating whether the todo is active (not archived).
    /// </summary>
    bool IsActive,
    
    /// <summary>
    /// Gets the priority level (1=Low, 2=Medium, 3=High, 4=Critical).
    /// </summary>
    int Priority,
    
    /// <summary>
    /// Gets the optional due date for the todo.
    /// </summary>
    DateTimeOffset? DueDate,
    
    /// <summary>
    /// Gets a value indicating whether the todo has been completed.
    /// </summary>
    bool IsCompleted,
    
    /// <summary>
    /// Gets the timestamp when the todo was completed (if IsCompleted is true).
    /// </summary>
    DateTimeOffset? CompletedAt,
    
    /// <summary>
    /// Gets the total number of tasks associated with this todo.
    /// </summary>
    int TaskCount,
    
    /// <summary>
    /// Gets the number of completed tasks within this todo.
    /// </summary>
    int CompletedTaskCount,
    
    /// <summary>
    /// Gets the timestamp when the todo was created.
    /// </summary>
    DateTimeOffset CreatedOnUtc,
    
    /// <summary>
    /// Gets the username of the person who created the todo.
    /// </summary>
    string? CreatedByUserName);

/// <summary>
/// Summary data transfer object for a Todo item.
/// 
/// **Purpose:**
/// Contains essential information about a todo for list/summary views.
/// Lighter weight than TodoDto, used for paginated list responses.
/// 
/// **Properties:**
/// Includes key identification, status, and task completion metrics.
/// </summary>
public record TodoSummaryDto(
    /// <summary>
    /// Gets the unique identifier of the todo.
    /// </summary>
    Guid Id,
    
    /// <summary>
    /// Gets the name/title of the todo.
    /// </summary>
    string Name,
    
    /// <summary>
    /// Gets the optional description.
    /// </summary>
    string? Description,
    
    /// <summary>
    /// Gets the current status of the todo.
    /// </summary>
    string Status,
    
    /// <summary>
    /// Gets the priority level (1=Low, 2=Medium, 3=High, 4=Critical).
    /// </summary>
    int Priority,
    
    /// <summary>
    /// Gets the optional due date.
    /// </summary>
    DateTimeOffset? DueDate,
    
    /// <summary>
    /// Gets a value indicating whether the todo is completed.
    /// </summary>
    bool IsCompleted,
    
    /// <summary>
    /// Gets the total number of tasks.
    /// </summary>
    int TaskCount,
    
    /// <summary>
    /// Gets the number of completed tasks.
    /// </summary>
    int CompletedTaskCount);
