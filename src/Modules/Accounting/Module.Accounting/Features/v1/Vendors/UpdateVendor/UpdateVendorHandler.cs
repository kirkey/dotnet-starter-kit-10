using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Vendors.UpdateVendor;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Vendors.UpdateVendor;

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
