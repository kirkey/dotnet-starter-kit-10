using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.DeleteCreditMemo;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.DeleteCreditMemo;

public class DeleteCreditMemoHandler(AccountingDbContext context) : ICommandHandler<DeleteCreditMemoCommand>
{
    public async ValueTask<Unit> Handle(DeleteCreditMemoCommand command, CancellationToken ct)
    {
        var entity = await context.CreditMemos.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("CreditMemo not found");
        
        context.CreditMemos.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
