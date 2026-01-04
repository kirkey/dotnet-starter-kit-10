using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountingPeriods.UpdateAccountingPeriod;

public record UpdateAccountingPeriodCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

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
