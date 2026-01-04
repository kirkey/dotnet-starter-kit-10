using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.UpdateChartOfAccount;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.UpdateChartOfAccount;

/// <summary>
/// Handler for updating a Chart of Account.
/// 
/// **Responsibility:**
/// Updates an existing Chart of Account with new values after validation.
/// 
/// **Execution Flow:**
/// 1. Find ChartOfAccount by ID, throw NotFoundException if not found
/// 2. Call entity.Update() domain method with new values
/// 3. Persist changes to database
/// 4. Return the updated account ID
/// 
/// **Updateable Fields:**
/// - AccountCode: Can be updated if remains unique
/// - AccountName: Account description
/// - AccountType: Account classification
/// - ParentAccountId: Account hierarchy parent
/// - Balance: Account balance
/// - IsControlAccount: Control account flag
/// - NormalBalance: Debit or Credit normal balance
/// - RegulatoryClassification: Regulatory category
/// - Description, Notes: Descriptive fields
/// 
/// **Immutable Fields:**
/// - Id: Cannot change after creation
/// - Original creation information
/// 
/// **Permissions:**
/// Requires: Accounting.ChartOfAccount.Edit
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when account is not found
/// </summary>
public class UpdateChartOfAccountHandler(AccountingDbContext context) : ICommandHandler<UpdateChartOfAccountCommand, Guid>
{
    /// <summary>
    /// Handles the UpdateChartOfAccountCommand to update account details.
    /// </summary>
    /// <param name="command">The command containing the Chart of Account updates</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ID of the updated Chart of Account</returns>
    /// <exception cref="NotFoundException">Thrown when ChartOfAccount is not found</exception>
    public async ValueTask<Guid> Handle(UpdateChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = await context.ChartOfAccounts.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("ChartOfAccount not found");
        
        entity.Update(
            command.AccountCode,
            command.AccountName,
            command.AccountType,
            command.UsoaCategory,
            command.ParentAccountId,
            command.ParentCode,
            command.Balance,
            command.IsControlAccount,
            command.NormalBalance,
            command.IsUsoaCompliant,
            command.RegulatoryClassification,
            command.Description,
            command.Notes);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
