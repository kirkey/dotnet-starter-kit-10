using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSegments.CreateCustomerSegment;

public record CreateCustomerSegmentCommand(string Name) : ICommand<Guid>;

public class CreateCustomerSegmentHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCustomerSegmentCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCustomerSegmentCommand command, CancellationToken ct)
    {
        var entity = CustomerSegment.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CustomerSegments.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
