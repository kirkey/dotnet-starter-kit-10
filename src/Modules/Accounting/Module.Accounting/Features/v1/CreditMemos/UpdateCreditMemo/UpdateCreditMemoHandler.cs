using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.UpdateCreditMemo;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.UpdateCreditMemo;

public class UpdateCreditMemoHandler(AccountingDbContext context) : ICommandHandler<UpdateCreditMemoCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCreditMemoCommand command, CancellationToken ct)
    {
        var entity = await context.CreditMemos.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("CreditMemo not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
