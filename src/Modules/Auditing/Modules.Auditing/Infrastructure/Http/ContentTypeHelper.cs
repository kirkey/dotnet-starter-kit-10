using Microsoft.Net.Http.Headers;

namespace FSH.Modules.Auditing.Infrastructure.Http;

internal static class ContentTypeHelper
{
    public static bool IsJsonLike(string? contentType, ISet<string> allowed)
    {
        if (string.IsNullOrWhiteSpace(contentType)) return false;

        // Prefer robust parse; fallback to naive split if needed.
        if (MediaTypeHeaderValue.TryParse(contentType, out MediaTypeHeaderValue? mt))
            return allowed.Contains(mt.MediaType.Value ?? string.Empty);

        int semi = contentType.IndexOf(';', StringComparison.Ordinal);
        string type = semi >= 0 ? contentType[..semi] : contentType;
        return allowed.Contains(type.Trim());
    }
}

