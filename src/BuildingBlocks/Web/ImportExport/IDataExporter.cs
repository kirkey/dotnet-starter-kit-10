namespace FSH.Framework.Web.ImportExport;

/// <summary>
/// Interface for exporting data to various formats
/// </summary>
/// <typeparam name="T">Type of data to export</typeparam>
public interface IDataExporter<T>
{
    /// <summary>
    /// Export data to a byte array in the specified format
    /// </summary>
    Task<ExportResult> ExportAsync(IEnumerable<T> data, string? fileName = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the content type for the export format
    /// </summary>
    string ContentType { get; }
    
    /// <summary>
    /// Gets the file extension for the export format
    /// </summary>
    string FileExtension { get; }
}

/// <summary>
/// Result of an export operation
/// </summary>
public sealed class ExportResult
{
    public required byte[] Data { get; init; }
    public required string ContentType { get; init; }
    public required string FileName { get; init; }
}
