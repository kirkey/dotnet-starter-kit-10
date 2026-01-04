using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.BillLineItems.UpdateBillLineItem;

public record UpdateBillLineItemCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateBillLineItemHandler(AccountingDbContext context) : ICommandHandler<UpdateBillLineItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBillLineItemCommand command, CancellationToken ct)
    {
        var entity = await context.BillLineItems.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("BillLineItem not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
