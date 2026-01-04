using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Vendors.DeleteVendor;

public record DeleteVendorCommand(Guid Id) : ICommand;

public class DeleteVendorHandler(AccountingDbContext context) : ICommandHandler<DeleteVendorCommand>
{
    public async ValueTask<Unit> Handle(DeleteVendorCommand command, CancellationToken ct)
    {
        var entity = await context.Vendors.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Vendor not found");
        
        context.Vendors.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
