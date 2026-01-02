using System.Text.Json;

namespace FSH.Framework.Web.ImportExport;

/// <summary>
/// JSON exporter implementation
/// </summary>
public sealed class JsonDataExporter<T> : IDataExporter<T>
{
    private readonly JsonSerializerOptions _options;

    public string ContentType => "application/json";
    public string FileExtension => "json";

    public JsonDataExporter()
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public Task<ExportResult> ExportAsync(IEnumerable<T> data, string? fileName = null, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(data, _options);
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        
        var result = new ExportResult
        {
            Data = bytes,
            ContentType = ContentType,
            FileName = fileName ?? $"export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{FileExtension}"
        };
        
        return Task.FromResult(result);
    }
}
