// TODO: Implement Void operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Checks.VoidCheck;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.VoidCheck;

public class VoidCheckHandler(AccountingDbContext context) 
    : ICommandHandler<VoidCheckCommand>
{
    public async ValueTask<Unit> Handle(VoidCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct) ?? throw new NotFoundException("Check not found");
        entity.Void();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
