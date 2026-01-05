using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

using FSH.Module.Microfinance.Contracts.v1.PromiseToPays.CreatePromiseToPay;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.CreatePromiseToPay;

public class CreatePromiseToPayHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreatePromiseToPayCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePromiseToPayCommand command, CancellationToken ct)
    {
        var entity = PromiseToPay.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.PromiseToPays.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
