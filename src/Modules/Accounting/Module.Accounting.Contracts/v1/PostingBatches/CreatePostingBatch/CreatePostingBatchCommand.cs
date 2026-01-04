using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PostingBatches.CreatePostingBatch;

/// <summary>
/// Create Posting Batch command.
/// </summary>
public record CreatePostingBatchCommand(string Name, DateTime? BatchDate = null, string? Description = null) : ICommand<Guid>;
