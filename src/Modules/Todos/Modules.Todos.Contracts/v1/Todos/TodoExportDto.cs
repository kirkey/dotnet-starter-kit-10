namespace FSH.Modules.Todos.Contracts.v1.Todos;

/// <summary>
/// Data transfer object for exporting todos to external formats.
/// 
/// **Purpose:**
/// Used for bulk export operations to serialize todos to external formats
/// (CSV files, JSON, Excel, etc.).
/// 
/// **Properties:**
/// Contains all essential todo information needed for external representation
/// including completion status and timestamps.
/// </summary>
public record TodoExportDto(
    /// <summary>
    /// Gets the name/title of the todo.
    /// </summary>
    string Name,
    
    /// <summary>
    /// Gets the optional description.
    /// </summary>
    string? Description,
    
    /// <summary>
    /// Gets the optional notes.
    /// </summary>
    string? Notes,
    
    /// <summary>
    /// Gets the status of the todo (NotStarted, InProgress, Completed, OnHold).
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
    /// Gets the timestamp when the todo was completed (if IsCompleted is true).
    /// </summary>
    DateTimeOffset? CompletedAt);
