namespace Accounting.Application.PostingBatches.Search.v1;

public sealed record PostingBatchSearchResponse
{
    public DefaultIdType Id { get; init; }
    public string BatchNumber { get; init; } = string.Empty;
    public DateTime BatchDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string ApprovalStatus { get; init; } = string.Empty;
    public string? Description { get; init; }
    public int EntryCount { get; init; }  // Changed from JournalEntryCount to match entity property
    public DateTime CreatedOn { get; init; }
}
