// TODO: Implement Clear operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Checks.ClearCheck;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.ClearCheck;

public class ClearCheckHandler(AccountingDbContext context) 
    : ICommandHandler<ClearCheckCommand>
{
    public async ValueTask<Unit> Handle(ClearCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct) ?? throw new NotFoundException("Check not found");
        // For now, clearedBy is set to Guid.Empty. In future we may pass current user id.
        entity.Clear(Guid.Empty);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
