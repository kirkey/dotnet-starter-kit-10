using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CustomerSegments.UpdateCustomerSegment;

namespace FSH.Module.Microfinance.Features.v1.CustomerSegments.UpdateCustomerSegment;

public class UpdateCustomerSegmentHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCustomerSegmentCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCustomerSegmentCommand command, CancellationToken ct)
    {
        var entity = await context.CustomerSegments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CustomerSegment not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
