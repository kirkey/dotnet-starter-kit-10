using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.AccountingPeriods.CreateAccountingPeriod;

public record CreateAccountingPeriodCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateAccountingPeriodHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateAccountingPeriodCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateAccountingPeriodCommand command, CancellationToken ct)
    {
        var entity = AccountingPeriod.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.AccountingPeriods.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
