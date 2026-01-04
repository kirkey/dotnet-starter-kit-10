using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountReconciliations.CreateAccountReconciliation;

public record CreateAccountReconciliationCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateAccountReconciliationHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateAccountReconciliationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAccountReconciliationCommand command, CancellationToken ct)
    {
        var entity = AccountReconciliation.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.AccountReconciliations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
