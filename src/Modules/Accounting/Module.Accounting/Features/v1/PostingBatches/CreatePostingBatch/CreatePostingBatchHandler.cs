using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.CreatePostingBatch;
using FSH.Module.Accounting.Domain;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.CreatePostingBatch;

public class CreatePostingBatchHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePostingBatchCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePostingBatchCommand command, CancellationToken ct)
    {
        var batchDate = command.BatchDate ?? DateTime.UtcNow.Date;
        var entity = PostingBatch.Create(
            command.Name,
            batchDate,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.PostingBatches.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
