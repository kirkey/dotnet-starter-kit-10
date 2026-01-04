using FSH.Framework.Core.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.CreateBankReconciliation;

public record CreateBankReconciliationCommand(
    string ReconciliationNumber,
    Guid BankAccountId,
    DateTime StatementDate,
    decimal StatementBalance,
    decimal BookBalance,
    string? Description = null) : ICommand<Guid>;

public class CreateBankReconciliationHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateBankReconciliationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateBankReconciliationCommand command, CancellationToken ct)
    {
        var entity = BankReconciliation.Create(
            command.ReconciliationNumber,
            command.BankAccountId,
            command.StatementDate,
            command.StatementBalance,
            command.BookBalance,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.BankReconciliations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
