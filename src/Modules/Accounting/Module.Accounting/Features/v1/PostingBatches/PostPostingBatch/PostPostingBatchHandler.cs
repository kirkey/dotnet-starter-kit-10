using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.PostPostingBatch;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.PostPostingBatch;

public class PostPostingBatchHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<PostPostingBatchCommand>
{
    public async ValueTask<Unit> Handle(PostPostingBatchCommand command, CancellationToken ct)
    {
        var entity = await context.PostingBatches.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("PostingBatch not found");

        // Business rules are enforced in domain
        entity.Post(currentUser.GetUserId());

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
