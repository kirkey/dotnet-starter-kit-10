using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.AccountingPeriods.UpdateAccountingPeriod;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.UpdateAccountingPeriod;

public class UpdateAccountingPeriodHandler(AccountingDbContext context) : ICommandHandler<UpdateAccountingPeriodCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateAccountingPeriodCommand command, CancellationToken ct)
    {
        var entity = await context.AccountingPeriods.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("AccountingPeriod not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
