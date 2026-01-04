using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Bills.UpdateBill;

public record UpdateBillCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateBillHandler(AccountingDbContext context) : ICommandHandler<UpdateBillCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBillCommand command, CancellationToken ct)
    {
        var entity = await context.Bills.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bill not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
