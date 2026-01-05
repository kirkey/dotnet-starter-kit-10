using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PostingBatches.RejectPostingBatch;

/// <summary>
/// Reject Posting Batch command.
/// </summary>
public record RejectPostingBatchCommand(Guid Id) : ICommand;