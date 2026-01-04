using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.ApprovePostingBatch;

public record ApprovePostingBatchCommand(Guid Id) : ICommand;

public class ApprovePostingBatchHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<ApprovePostingBatchCommand>
{
    public async ValueTask<Unit> Handle(ApprovePostingBatchCommand command, CancellationToken ct)
    {
        var entity = await context.PostingBatches.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("PostingBatch not found");

        entity.Approve(currentUser.GetUserId());

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
