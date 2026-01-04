using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Consumption.DeleteConsumption;

public record DeleteConsumptionCommand(Guid Id) : ICommand;

public class DeleteConsumptionHandler(AccountingDbContext context) : ICommandHandler<DeleteConsumptionCommand>
{
    public async ValueTask<Unit> Handle(DeleteConsumptionCommand command, CancellationToken ct)
    {
        var entity = await context.Consumption.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Consumption not found");

        // Business rule: Cannot delete consumption with associated invoices
        var hasInvoices = await context.Invoices.AnyAsync(x => x.ConsumptionId == command.Id, ct).ConfigureAwait(false);
        if (hasInvoices)
            throw new BadRequestException("Cannot delete consumption with existing invoices. Please remove or reassign invoices first.");
        
        context.Consumption.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
