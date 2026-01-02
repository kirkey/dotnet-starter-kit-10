using System.Text.Json;

namespace FSH.Framework.Web.ImportExport;

/// <summary>
/// JSON importer implementation
/// </summary>
public sealed class JsonDataImporter<T> : IDataImporter<T>
{
    private readonly JsonSerializerOptions _options;

    public IReadOnlyList<string> SupportedContentTypes => new[] { "application/json", "text/json" };

    public JsonDataImporter()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<ImportResult<T>> ImportAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await JsonSerializer.DeserializeAsync<List<T>>(stream, _options, cancellationToken);
            
            if (data == null)
            {
                return new ImportResult<T>
                {
                    Data = Array.Empty<T>(),
                    SuccessCount = 0,
                    FailureCount = 0,
                    Errors = new[] { "Failed to deserialize JSON data" }
                };
            }

            return new ImportResult<T>
            {
                Data = data,
                SuccessCount = data.Count,
                FailureCount = 0,
                Errors = Array.Empty<string>()
            };
        }
        catch (JsonException ex)
        {
            return new ImportResult<T>
            {
                Data = Array.Empty<T>(),
                SuccessCount = 0,
                FailureCount = 1,
                Errors = new[] { $"JSON parsing error: {ex.Message}" }
            };
        }
    }
}
