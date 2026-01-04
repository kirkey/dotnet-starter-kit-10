using FSH.Module.Accounting.Contracts.v1.PostingBatches;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PostingBatches.GetListPostingBatch;

/// <summary>
/// Get Posting Batches (paginated) query.
/// </summary>
public record GetPostingBatchesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<PostingBatchesPagedResponse>;

/// <summary>
/// Paginated response for posting batches listing.
/// </summary>
public record PostingBatchesPagedResponse(
    List<PostingBatchSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for posting batch list items.
/// </summary>
public record PostingBatchSummaryDto(Guid Id, string Name, string? Status, bool IsActive);
