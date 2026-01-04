using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PostingBatches.ApprovePostingBatch;

/// <summary>
/// Approve Posting Batch command.
/// </summary>
public record ApprovePostingBatchCommand(Guid Id) : ICommand;
