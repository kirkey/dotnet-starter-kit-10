namespace FSH.Modules.Ai.Services;

public sealed record FetchedPage(string FinalUrl, int StatusCode, string ContentType, string Html);

public interface IWebPageFetcher
{
    Task<FetchedPage> FetchAsync(string url, CancellationToken ct = default);
}

/// <summary>
/// Fetches a tenant-supplied URL for ingestion. Guards: http(s) only, no redirects, 10 MB cap,
/// resilient HTTP (retry/timeout via AddHeroResilience). Non-HTML content throws with a reason
/// the caller records as a failed ingestion.
/// </summary>
public sealed class WebPageFetcher(IHttpClientFactory httpFactory) : IWebPageFetcher
{
    private const long MaxBytes = 10 * 1024 * 1024;

    public async Task<FetchedPage> FetchAsync(string url, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(url);
        if (!Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri))
        {
            throw new InvalidOperationException($"'{url}' is not a valid absolute URL.");
        }

        AiUrlGuard.Validate(uri);

        var client = httpFactory.CreateClient("AiWebFetch");
        using var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Fetching '{uri}' failed with status {(int)response.StatusCode}.");
        }

        var mediaType = response.Content.Headers.ContentType?.MediaType ?? string.Empty;
        if (!mediaType.Contains("html", StringComparison.OrdinalIgnoreCase)
            && !mediaType.Contains("text", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"'{uri}' returned '{mediaType}', which cannot be converted to readable Markdown.");
        }

        using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var buffer = new MemoryStream();
        var remaining = MaxBytes + 1;
        var chunk = new byte[81920];
        int read;
        while ((read = await stream.ReadAsync(chunk, ct).ConfigureAwait(false)) > 0)
        {
            remaining -= read;
            if (remaining <= 0)
            {
                throw new InvalidOperationException($"'{uri}' exceeds the {MaxBytes} byte fetch limit.");
            }

            await buffer.WriteAsync(chunk.AsMemory(0, read), ct).ConfigureAwait(false);
        }

        buffer.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(buffer, System.Text.Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
        var html = await reader.ReadToEndAsync(ct).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(html))
        {
            throw new InvalidOperationException($"'{uri}' returned no readable content.");
        }

        return new FetchedPage(uri.ToString(), (int)response.StatusCode, mediaType, html);
    }
}
