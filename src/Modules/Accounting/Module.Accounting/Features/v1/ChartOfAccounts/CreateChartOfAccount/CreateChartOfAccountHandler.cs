using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.CreateChartOfAccount;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.CreateChartOfAccount;

/// <summary>
/// Handles the creation of a new Chart of Account.
/// 
/// **Purpose:**
/// Processes the CreateChartOfAccountCommand by:
/// 1. Creating a new ChartOfAccount aggregate from the command data
/// 2. Setting parent account relationships if specified
/// 3. Recording initial balance
/// 4. Persisting to the database
/// 5. Raising domain event (if implemented)
/// 6. Returning the new account's ID
/// 
/// **Domain Logic:**
/// - Uses the ChartOfAccount.Create factory method to ensure proper initialization
/// - Maintains account hierarchy through parent account references
/// - Validates account type and classification
/// - Associates account with the current user and tenant
/// - Maintains audit trail through creation tracking
/// - Supports USOA (Utility System of Accounts) compliance
/// 
/// **Dependencies:**
/// - AccountingDbContext: For database persistence
/// - ICurrentUser: For accessing current user context (ID, username, tenant)
/// </summary>
public class CreateChartOfAccountHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateChartOfAccountCommand, Guid>
{
    /// <summary>
    /// Handles the CreateChartOfAccountCommand to create a new account.
    /// </summary>
    /// <param name="command">The command containing account creation details.</param>
    /// <param name="ct">Cancellation token for the operation.</param>
    /// <returns>The ID of the newly created account.</returns>
    public async ValueTask<Guid> Handle(CreateChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = ChartOfAccount.Create(
            command.AccountCode,
            command.AccountName,
            command.AccountType,
            command.UsoaCategory,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.ParentAccountId,
            command.ParentCode,
            command.Balance,
            command.IsControlAccount,
            command.NormalBalance,
            command.IsUsoaCompliant,
            command.RegulatoryClassification,
            command.Description,
            command.Notes);
        
        context.ChartOfAccounts.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
