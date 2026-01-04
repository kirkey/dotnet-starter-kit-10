using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Checks.DeleteCheck;

public record DeleteCheckCommand(Guid Id) : ICommand;

public class DeleteCheckHandler(AccountingDbContext context) : ICommandHandler<DeleteCheckCommand>
{
    public async ValueTask<Unit> Handle(DeleteCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Check not found");

        context.Checks.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}