namespace FSH.Framework.Blazor.UI.Components.Data.EntityTable;

/// <summary>
/// Represents the result of an import operation.
/// </summary>
public sealed class ImportResponse
{
    /// <summary>
    /// Number of records successfully imported.
    /// </summary>
    public int ImportedCount { get; set; }
    
    /// <summary>
    /// Number of records that failed to import.
    /// </summary>
    public int FailedCount { get; set; }
    
    /// <summary>
    /// Total number of records processed.
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Whether the import was successful (no failures).
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// Collection of error messages for failed records.
    /// </summary>
    public ICollection<string>? Errors { get; set; }
}
