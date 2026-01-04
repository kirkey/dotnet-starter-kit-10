using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.BillLineItems.CreateBillLineItem;

public record CreateBillLineItemCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateBillLineItemHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBillLineItemCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBillLineItemCommand command, CancellationToken ct)
    {
        var entity = BillLineItem.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.BillLineItems.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
