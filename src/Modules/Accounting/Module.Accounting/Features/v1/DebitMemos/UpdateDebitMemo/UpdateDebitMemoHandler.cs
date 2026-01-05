using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.DebitMemos.UpdateDebitMemo;

namespace FSH.Module.Accounting.Features.v1.DebitMemos.UpdateDebitMemo;

public class UpdateDebitMemoHandler(AccountingDbContext context) : ICommandHandler<UpdateDebitMemoCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateDebitMemoCommand command, CancellationToken ct)
    {
        var entity = await context.DebitMemos.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("DebitMemo not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
