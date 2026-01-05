using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.CustomerSegments.CreateCustomerSegment;

namespace FSH.Module.Microfinance.Features.v1.CustomerSegments.CreateCustomerSegment;

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
