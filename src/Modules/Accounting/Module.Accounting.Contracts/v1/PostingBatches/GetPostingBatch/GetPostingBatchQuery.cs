using FSH.Module.Accounting.Contracts.v1.PostingBatches;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PostingBatches.GetPostingBatch;

/// <summary>
/// Get Posting Batch query.
/// </summary>
public record GetPostingBatchQuery(Guid Id) : IQuery<PostingBatchDto>;