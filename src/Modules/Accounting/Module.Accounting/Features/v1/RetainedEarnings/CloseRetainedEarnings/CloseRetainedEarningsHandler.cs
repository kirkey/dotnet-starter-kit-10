using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CloseRetainedEarnings;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.CloseRetainedEarnings; 

public class CloseRetainedEarningsHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CloseRetainedEarningsCommand>
{
    public async ValueTask<Unit> Handle(CloseRetainedEarningsCommand command, CancellationToken ct)
    {
        var entity = await context.RetainedEarnings.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("RetainedEarnings not found");

        entity.Close(command.FiscalYear, command.ClosingBalance, currentUser.GetUserId());

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
