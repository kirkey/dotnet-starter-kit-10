namespace FSH.Framework.Web.ImportExport;

/// <summary>
/// Interface for importing data from various formats
/// </summary>
/// <typeparam name="T">Type of data to import</typeparam>
public interface IDataImporter<T>
{
    /// <summary>
    /// Import data from a stream
    /// </summary>
    Task<ImportResult<T>> ImportAsync(Stream stream, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the supported content types for import
    /// </summary>
    IReadOnlyList<string> SupportedContentTypes { get; }
}

/// <summary>
/// Result of an import operation
/// </summary>
public sealed class ImportResult<T>
{
    public required IReadOnlyList<T> Data { get; init; }
    public required int SuccessCount { get; init; }
    public required int FailureCount { get; init; }
    public IReadOnlyList<string> Errors { get; init; } = Array.Empty<string>();
}
