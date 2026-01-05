using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.CreateBankReconciliation;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.CreateBankReconciliation;


/// <summary>
/// Handler for creating a BankReconciliation aggregate and persisting it.
/// </summary>
/// <remarks>
/// Responsibility: Initialize a bank reconciliation with statement and book balances and record audit context.
/// 
/// Execution Flow:
/// 1. Use BankReconciliation.Create() factory method with reconciliation details and current user context (tenant, userId, userName)
/// 2. Add new bank reconciliation to DbSet and persist via SaveChangesAsync
/// 3. Return created reconciliation Id for reference
/// 
/// Business Rules:
/// - ReconciliationNumber should be unique per tenant
/// - BankAccountId must reference an existing bank account
/// - StatementBalance and BookBalance are used to compute Difference for reconciliation
/// 
/// Multi-Tenancy & Audit: Tenant and created-by metadata provided by ICurrentUser
/// 
/// Permissions: Requires authenticated user with bank reconciliation creation permission
/// 
/// Exceptions:
/// - DbException: Thrown if foreign key constraint fails (BankAccountId not found)
/// - BadRequestException: Thrown by validators for invalid input
/// </remarks>
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
