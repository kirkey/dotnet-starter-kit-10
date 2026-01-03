using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSegments.DeleteCustomerSegment;

public record DeleteCustomerSegmentCommand(Guid Id) : ICommand;

public class DeleteCustomerSegmentHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCustomerSegmentCommand>
{
    public async ValueTask<Unit> Handle(DeleteCustomerSegmentCommand command, CancellationToken ct)
    {
        var entity = await context.CustomerSegments.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CustomerSegment not found");
        
        context.CustomerSegments.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
