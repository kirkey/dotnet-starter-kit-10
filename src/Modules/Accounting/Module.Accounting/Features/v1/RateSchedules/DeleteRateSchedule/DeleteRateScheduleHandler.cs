using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.RateSchedules.DeleteRateSchedule;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.RateSchedules.DeleteRateSchedule; 

public class DeleteRateScheduleHandler(AccountingDbContext context) : ICommandHandler<DeleteRateScheduleCommand>
{
    public async ValueTask<Unit> Handle(DeleteRateScheduleCommand command, CancellationToken ct)
    {
        var entity = await context.RateSchedules.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("RateSchedule not found");

        // Business rule: cannot delete a rate schedule that is assigned as a default to customers
        var isAssigned = await context.Customers.AnyAsync(c => c.DefaultRateScheduleId == command.Id, ct).ConfigureAwait(false);
        if (isAssigned)
            throw new BadRequestException("Cannot delete rate schedule that is assigned as default to one or more customers. Reassign or remove references first.");

        context.RateSchedules.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
} 
