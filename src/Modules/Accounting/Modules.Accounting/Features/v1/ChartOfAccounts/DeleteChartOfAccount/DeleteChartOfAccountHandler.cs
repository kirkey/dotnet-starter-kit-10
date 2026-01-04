using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.ChartOfAccounts.DeleteChartOfAccount;

public record DeleteChartOfAccountCommand(Guid Id) : ICommand;

public class DeleteChartOfAccountHandler(AccountingDbContext context) : ICommandHandler<DeleteChartOfAccountCommand>
{
    public async ValueTask<Unit> Handle(DeleteChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = await context.ChartOfAccounts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ChartOfAccount not found");
        
        context.ChartOfAccounts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
