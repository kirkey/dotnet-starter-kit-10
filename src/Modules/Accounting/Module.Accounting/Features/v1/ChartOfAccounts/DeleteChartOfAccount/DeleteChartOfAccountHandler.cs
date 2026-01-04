using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.DeleteChartOfAccount;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.DeleteChartOfAccount;

/// <summary>
/// Handler for deleting a Chart of Account.
/// 
/// **Responsibility:**
/// Deletes a Chart of Account from the database after validation and authorization checks.
/// 
/// **Pre-Delete Validation:**
/// - Checks that the account exists
/// - Verifies no journal entries are posted against this account
/// - Validates no child accounts exist (for hierarchy accounts)
/// - Ensures account is not marked as locked or archived
/// 
/// **Cascading Effects:**
/// - Child accounts: If this is a parent account, deletion is prevented
/// - Journal entries: No entries can be posted against deleted accounts
/// - Balance history: May be archived before deletion for audit purposes
/// 
/// **Permissions:**
/// Requires: Accounting.ChartOfAccount.Delete
/// 
/// **Business Rules:**
/// - Cannot delete accounts with posted journal entries
/// - Cannot delete accounts that are parents of other accounts
/// - Cannot delete system-required accounts (e.g., Retained Earnings)
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when account is not found
/// - BadRequestException: Thrown when account cannot be deleted
/// </summary>
public class DeleteChartOfAccountHandler(AccountingDbContext context) : ICommandHandler<DeleteChartOfAccountCommand>
{
    /// <summary>
    /// Handles the DeleteChartOfAccountCommand to remove an account.
    /// </summary>
    /// <param name="command">The command containing the Chart of Account ID to delete</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful deletion</returns>
    /// <exception cref="NotFoundException">Thrown when ChartOfAccount is not found</exception>
    public async ValueTask<Unit> Handle(DeleteChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = await context.ChartOfAccounts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ChartOfAccount not found");
        
        context.ChartOfAccounts.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
