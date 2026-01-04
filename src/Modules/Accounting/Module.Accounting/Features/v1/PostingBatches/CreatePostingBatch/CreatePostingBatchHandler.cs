using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.CreatePostingBatch;

public record CreatePostingBatchCommand(string Name, DateTime? BatchDate = null, string? Description = null) : ICommand<Guid>;

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
