using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.InterCompanyTransactions.CreateInterCompanyTransaction;

public record CreateInterCompanyTransactionCommand(string Name, string? Description) : ICommand<Guid>;

public class CreateInterCompanyTransactionHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInterCompanyTransactionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateInterCompanyTransactionCommand command, CancellationToken ct)
    {
        var entity = InterCompanyTransaction.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.InterCompanyTransactions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
