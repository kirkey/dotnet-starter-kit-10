using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.PromiseToPays.CreatePromiseToPay;

public record CreatePromiseToPayCommand(string Name) : ICommand<Guid>;

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
