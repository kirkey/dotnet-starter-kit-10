using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.RetainedEarnings.ReopenRetainedEarnings;

public record ReopenRetainedEarningsCommand(Guid Id, string? Reason = null) : ICommand;

public class ReopenRetainedEarningsHandler(AccountingDbContext context) 
    : ICommandHandler<ReopenRetainedEarningsCommand>
{
    public async ValueTask<Unit> Handle(ReopenRetainedEarningsCommand command, CancellationToken ct)
    {
        var entity = await context.RetainedEarnings.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("RetainedEarnings not found");

        entity.Reopen(command.Reason ?? string.Empty);

        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
