using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.PromiseToPays.UpdatePromiseToPay;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.UpdatePromiseToPay;

public class UpdatePromiseToPayHandler(MicrofinanceDbContext context) : ICommandHandler<UpdatePromiseToPayCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdatePromiseToPayCommand command, CancellationToken ct)
    {
        var entity = await context.PromiseToPays.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("PromiseToPay not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
