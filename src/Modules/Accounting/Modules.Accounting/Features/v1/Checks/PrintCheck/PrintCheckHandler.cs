// TODO: Implement Print operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Checks.PrintCheck;

public record PrintCheckCommand(Guid Id) : ICommand;

public class PrintCheckHandler(AccountingDbContext context) 
    : ICommandHandler<PrintCheckCommand>
{
    public async ValueTask<Unit> Handle(PrintCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct) ?? throw new NotFoundException("Check not found");
        entity.Print();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
