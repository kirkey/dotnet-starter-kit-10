using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.PrepaidExpenses.CreatePrepaidExpense;

public record CreatePrepaidExpenseCommand(string Name, string? Description) : ICommand<Guid>;

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
