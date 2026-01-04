using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Bills.CreateBill;

public record CreateBillCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateBillHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBillCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBillCommand command, CancellationToken ct)
    {
        var entity = Bill.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Bills.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
