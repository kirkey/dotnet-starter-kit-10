// TODO: Implement Issue operation for Check
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.Checks.IssueCheck;

public record IssueCheckCommand(Guid Id) : ICommand;

public class IssueCheckHandler(AccountingDbContext context) 
    : ICommandHandler<IssueCheckCommand>
{
    public async ValueTask<Unit> Handle(IssueCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct) ?? throw new NotFoundException("Check not found");
        entity.Issue();
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
