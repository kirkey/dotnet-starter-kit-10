using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Vendors.UpdateVendor;

public record UpdateVendorCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class UpdateVendorHandler(AccountingDbContext context) : ICommandHandler<UpdateVendorCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateVendorCommand command, CancellationToken ct)
    {
        var entity = await context.Vendors.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Vendor not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
