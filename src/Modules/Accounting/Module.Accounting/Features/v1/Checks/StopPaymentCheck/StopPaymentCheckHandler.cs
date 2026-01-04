// TODO: Implement StopPayment operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.StopPaymentCheck;

public record StopPaymentCheckCommand(Guid Id) : ICommand;

public class StopPaymentCheckHandler(AccountingDbContext context) 
    : ICommandHandler<StopPaymentCheckCommand>
{
    public async ValueTask<Unit> Handle(StopPaymentCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct) ?? throw new NotFoundException("Check not found");
        entity.StopPayment();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
