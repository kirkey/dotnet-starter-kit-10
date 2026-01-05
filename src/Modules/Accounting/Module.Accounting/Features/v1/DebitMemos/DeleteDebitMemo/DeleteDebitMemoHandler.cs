using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.DebitMemos.DeleteDebitMemo;

namespace FSH.Module.Accounting.Features.v1.DebitMemos.DeleteDebitMemo;

public class DeleteDebitMemoHandler(AccountingDbContext context) : ICommandHandler<DeleteDebitMemoCommand>
{
    public async ValueTask<Unit> Handle(DeleteDebitMemoCommand command, CancellationToken ct)
    {
        var entity = await context.DebitMemos.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("DebitMemo not found");
        
        context.DebitMemos.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
