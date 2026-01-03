using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSegments.UpdateCustomerSegment;

public record UpdateCustomerSegmentCommand(Guid Id, string Name) : ICommand<Guid>;

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
