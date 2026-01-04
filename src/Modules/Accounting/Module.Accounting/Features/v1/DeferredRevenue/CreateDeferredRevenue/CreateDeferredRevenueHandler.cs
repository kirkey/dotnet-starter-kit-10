using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.DeferredRevenue.CreateDeferredRevenue;

public record CreateDeferredRevenueCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateDeferredRevenueHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateDeferredRevenueCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateDeferredRevenueCommand command, CancellationToken ct)
    {
        var entity = DeferredRevenue.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.DeferredRevenue.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
