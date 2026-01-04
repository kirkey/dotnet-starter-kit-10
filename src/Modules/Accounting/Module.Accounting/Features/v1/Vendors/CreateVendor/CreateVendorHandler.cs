using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Vendors.CreateVendor;

public record CreateVendorCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateVendorHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateVendorCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateVendorCommand command, CancellationToken ct)
    {
        var entity = Vendor.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.Vendors.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
