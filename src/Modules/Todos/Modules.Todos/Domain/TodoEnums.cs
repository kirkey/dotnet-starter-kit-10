namespace FSH.Modules.Todos.Domain;

/// <summary>
/// Represents the current status of a Todo item.
/// 
/// Used to track the workflow state of a todo throughout its lifecycle.
/// </summary>
public enum TodoStatus
{
    /// <summary>
    /// Todo has been created but work has not started.
    /// </summary>
    NotStarted = 0,
    
    /// <summary>
    /// Todo work is currently in progress.
    /// </summary>
    InProgress = 1,
    
    /// <summary>
    /// Todo work has been completed.
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Todo work is temporarily on hold.
    /// </summary>
    OnHold = 3
}

/// <summary>
/// Represents the priority level of a Todo item.
/// 
/// Used to prioritize todos and help with task planning and allocation.
/// Higher priority values indicate greater urgency.
/// </summary>
public enum TodoPriority
{
    /// <summary>
    /// No priority level specified.
    /// </summary>
    None,
    
    /// <summary>
    /// Low priority - can be deferred if needed.
    /// </summary>
    Low = 1,
    
    /// <summary>
    /// Medium priority - should be addressed in regular workflow.
    /// </summary>
    Medium = 2,
    
    /// <summary>
    /// High priority - should be prioritized.
    /// </summary>
    High = 3,
    
    /// <summary>
    /// Critical priority - must be addressed immediately.
    /// </summary>
    Critical = 4
}
