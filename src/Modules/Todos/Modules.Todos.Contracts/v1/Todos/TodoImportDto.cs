namespace FSH.Modules.Todos.Contracts.v1.Todos;

/// <summary>
/// Data transfer object for importing todos from external sources.
/// 
/// **Purpose:**
/// Used for bulk import operations to create multiple todos from external data sources
/// (CSV files, JSON, etc.).
/// 
/// **Properties:**
/// - Name: The todo title (required)
/// - Description: Optional detailed description
/// - Notes: Optional notes
/// - Status: The status as a string (defaults to "NotStarted")
/// - Priority: Priority level (1-4)
/// - DueDate: Optional due date
/// 
/// **Validation:**
/// Name is required. Other properties are optional with sensible defaults.
/// </summary>
public class TodoImportDto
{
    /// <summary>
    /// Gets or sets the name/title of the todo to import (required).
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the optional description.
    /// </summary>
    public string? Description { get; init; }
    
    /// <summary>
    /// Gets or sets the optional notes.
    /// </summary>
    public string? Notes { get; init; }
    
    /// <summary>
    /// Gets or sets the status as a string (defaults to "NotStarted").
    /// Valid values: NotStarted, InProgress, Completed, OnHold
    /// </summary>
    public string Status { get; init; } = "NotStarted";
    
    /// <summary>
    /// Gets or sets the priority level (1=Low, 2=Medium, 3=High, 4=Critical).
    /// </summary>
    public int Priority { get; init; }
    
    /// <summary>
    /// Gets or sets the optional due date.
    /// </summary>
    public DateTimeOffset? DueDate { get; init; }
}
