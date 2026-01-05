using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.CreatePrepaidExpense;

namespace FSH.Module.Accounting.Features.v1.PrepaidExpenses.CreatePrepaidExpense;

public class CreatePrepaidExpenseHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreatePrepaidExpenseCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreatePrepaidExpenseCommand command, CancellationToken ct)
    {
        var entity = PrepaidExpense.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.PrepaidExpenses.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
