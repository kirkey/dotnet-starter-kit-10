namespace FSH.Framework.Web.Idempotency;

/// <summary>
/// A cached HTTP response for idempotent replay. Serialized to JSON bytes and stored via
/// <c>IDistributedCache</c> directly (both write and probe read); never through HybridCache,
/// whose namespaced L2 keys are invisible to raw reads.
/// </summary>
public sealed record CachedIdempotentResponse
{
    public int StatusCode { get; init; }
    public string? ContentType { get; init; }
    public byte[] Body { get; init; } = [];
}
