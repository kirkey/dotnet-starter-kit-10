using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.RejectPostingBatch;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.RejectPostingBatch;

public class RejectPostingBatchHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<RejectPostingBatchCommand>
{
    public async ValueTask<Unit> Handle(RejectPostingBatchCommand command, CancellationToken ct)
    {
        var entity = await context.PostingBatches.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("PostingBatch not found");

        entity.Reject(currentUser.GetUserId());

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
