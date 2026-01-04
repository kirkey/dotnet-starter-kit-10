// TODO: Implement Clear operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Checks.ClearCheck;

public record ClearCheckCommand(Guid Id) : ICommand;

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
