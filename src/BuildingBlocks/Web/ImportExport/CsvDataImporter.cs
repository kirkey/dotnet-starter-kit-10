using System.Globalization;
using System.Text;

namespace FSH.Framework.Web.ImportExport;

/// <summary>
/// CSV importer implementation
/// </summary>
public sealed class CsvDataImporter<T> : IDataImporter<T> where T : new()
{
    public IReadOnlyList<string> SupportedContentTypes => new[] { "text/csv", "application/csv" };

    public async Task<ImportResult<T>> ImportAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        using var reader = new StreamReader(stream, Encoding.UTF8);
        var data = new List<T>();
        var errors = new List<string>();
        var properties = typeof(T).GetProperties().Where(p => p.CanWrite).ToArray();

        // Read header
        var headerLine = await reader.ReadLineAsync(cancellationToken);
        if (string.IsNullOrWhiteSpace(headerLine))
        {
            return new ImportResult<T>
            {
                Data = Array.Empty<T>(),
                SuccessCount = 0,
                FailureCount = 0,
                Errors = new[] { "CSV file is empty or has no header" }
            };
        }

        var headers = ParseCsvLine(headerLine);
        var propertyMap = new Dictionary<int, System.Reflection.PropertyInfo>();

        for (int i = 0; i < headers.Length; i++)
        {
            var property = properties.FirstOrDefault(p => 
                p.Name.Equals(headers[i], StringComparison.OrdinalIgnoreCase));
            if (property != null)
            {
                propertyMap[i] = property;
            }
        }

        int lineNumber = 1;
        while (!reader.EndOfStream)
        {
            lineNumber++;
            var line = await reader.ReadLineAsync(cancellationToken);
            if (string.IsNullOrWhiteSpace(line)) continue;

            try
            {
                var values = ParseCsvLine(line);
                var item = new T();

                foreach (var kvp in propertyMap)
                {
                    if (kvp.Key < values.Length)
                    {
                        var value = values[kvp.Key];
                        if (!string.IsNullOrEmpty(value))
                        {
                            try
                            {
                                var convertedValue = Convert.ChangeType(value, 
                                    Nullable.GetUnderlyingType(kvp.Value.PropertyType) ?? kvp.Value.PropertyType,
                                    CultureInfo.InvariantCulture);
                                kvp.Value.SetValue(item, convertedValue);
                            }
                            catch (Exception ex)
                            {
                                errors.Add($"Line {lineNumber}: Failed to convert '{value}' for property '{kvp.Value.Name}': {ex.Message}");
                            }
                        }
                    }
                }

                data.Add(item);
            }
            catch (Exception ex)
            {
                errors.Add($"Line {lineNumber}: {ex.Message}");
            }
        }

        return new ImportResult<T>
        {
            Data = data,
            SuccessCount = data.Count,
            FailureCount = errors.Count,
            Errors = errors
        };
    }

    private static string[] ParseCsvLine(string line)
    {
        var result = new List<string>();
        var currentField = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"')
            {
                if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                {
                    currentField.Append('"');
                    i++;
                }
                else
                {
                    inQuotes = !inQuotes;
                }
            }
            else if (c == ',' && !inQuotes)
            {
                result.Add(currentField.ToString());
                currentField.Clear();
            }
            else
            {
                currentField.Append(c);
            }
        }

        result.Add(currentField.ToString());
        return result.ToArray();
    }
}
