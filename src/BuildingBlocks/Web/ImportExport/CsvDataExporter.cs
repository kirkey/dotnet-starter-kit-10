using System.Text;

namespace FSH.Framework.Web.ImportExport;

/// <summary>
/// CSV exporter implementation
/// </summary>
public sealed class CsvDataExporter<T> : IDataExporter<T>
{
    public string ContentType => "text/csv";
    public string FileExtension => "csv";

    public Task<ExportResult> ExportAsync(IEnumerable<T> data, string? fileName = null, CancellationToken cancellationToken = default)
    {
        var csv = ConvertToCsv(data);
        var bytes = Encoding.UTF8.GetBytes(csv);
        
        var result = new ExportResult
        {
            Data = bytes,
            ContentType = ContentType,
            FileName = fileName ?? $"export_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{FileExtension}"
        };
        
        return Task.FromResult(result);
    }

    private static string ConvertToCsv(IEnumerable<T> data)
    {
        var sb = new StringBuilder();
        var properties = typeof(T).GetProperties();
        
        // Header
        sb.AppendLine(string.Join(",", properties.Select(p => EscapeCsvField(p.Name))));
        
        // Data rows
        foreach (var item in data)
        {
            var values = properties.Select(p =>
            {
                var value = p.GetValue(item);
                return EscapeCsvField(value?.ToString() ?? string.Empty);
            });
            sb.AppendLine(string.Join(",", values));
        }
        
        return sb.ToString();
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',', StringComparison.Ordinal) || 
            field.Contains('"', StringComparison.Ordinal) || 
            field.Contains('\n', StringComparison.Ordinal))
        {
            return $"\"{field.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";
        }
        return field;
    }
}
