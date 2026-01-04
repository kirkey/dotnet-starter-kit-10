using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.DeleteAmlAlert;

public record DeleteAmlAlertCommand(Guid Id) : ICommand;

public class DeleteAmlAlertHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteAmlAlertCommand>
{
    public async ValueTask<Unit> Handle(DeleteAmlAlertCommand command, CancellationToken ct)
    {
        var entity = await context.AmlAlerts.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("AmlAlert not found");
        
        context.AmlAlerts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
