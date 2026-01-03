using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.PromiseToPays.UpdatePromiseToPay;

public record UpdatePromiseToPayCommand(Guid Id, string Name) : ICommand<Guid>;

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
