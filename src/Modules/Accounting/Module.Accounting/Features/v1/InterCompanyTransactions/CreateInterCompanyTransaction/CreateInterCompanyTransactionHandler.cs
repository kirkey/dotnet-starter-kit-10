using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.CreateInterCompanyTransaction;

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
