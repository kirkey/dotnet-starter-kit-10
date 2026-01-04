using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PostingBatches.PostPostingBatch;

/// <summary>
/// Post Posting Batch command.
/// </summary>
public record PostPostingBatchCommand(Guid Id) : ICommand;
