namespace FSH.Module.Todos;

/// <summary>
/// Constants used throughout the Todo module for consistency.
/// 
/// Used for:
/// - Database schema naming
/// - API route prefixes
/// - Configuration keys
/// </summary>
public static class TodoModuleConstants
{
    /// <summary>
    /// Database schema name for all Todo-related tables.
    /// </summary>
    public const string SchemaName = "todo";
    
    /// <summary>
    /// Route prefix used for Todo API endpoints (e.g., /api/v1/todo).
    /// </summary>
    public const string Prefix = "todo";
}
