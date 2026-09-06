using FSH.Framework.Eventing.Abstractions;

namespace FSH.Modules.Files.Contracts.Events;

/// <summary>
/// Raised when a FileAsset transitions from PendingUpload to Available (or Quarantined). Owning
/// modules can subscribe to react to the upload completing — e.g. update a search index, send a
/// notification, etc.
/// StorageKey + OriginalFileName are carried so consumers (e.g. the Ai ingestion handler) can
/// download the original bytes without a cross-module lookup. Additive: messages serialized
/// before these fields existed deserialize with nulls, which consumers must tolerate.
/// </summary>
public sealed record FileFinalizedIntegrationEvent(
    Guid Id,
    DateTime OccurredOnUtc,
    string? TenantId,
    string CorrelationId,
    string Source,
    Guid FileAssetId,
    string OwnerType,
    Guid? OwnerId,
    string ContentType,
    long SizeBytes,
    int FinalStatus,
    string? StorageKey = null,
    string? OriginalFileName = null) : IIntegrationEvent;
