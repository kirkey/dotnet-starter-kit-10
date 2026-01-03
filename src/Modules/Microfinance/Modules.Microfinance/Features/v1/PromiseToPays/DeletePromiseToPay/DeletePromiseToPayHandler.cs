using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.PromiseToPays.DeletePromiseToPay;

public record DeletePromiseToPayCommand(Guid Id) : ICommand;

public class DeletePromiseToPayHandler(MicrofinanceDbContext context) : ICommandHandler<DeletePromiseToPayCommand>
{
    public async ValueTask<Unit> Handle(DeletePromiseToPayCommand command, CancellationToken ct)
    {
        var entity = await context.PromiseToPays.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("PromiseToPay not found");
        
        context.PromiseToPays.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
