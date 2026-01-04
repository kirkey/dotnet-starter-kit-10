using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.UpdateChartOfAccount;

/// <summary>
/// Update Chart of Account command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to update an existing Chart of Account with new values.
/// 
/// **Parameters:**
/// - Id: The Chart of Account ID to update
/// - AccountCode: New account code (must remain unique)
/// - AccountName: New account name
/// - AccountType: Updated account type classification
/// - UsoaCategory: Utility System of Accounts category
/// - ParentAccountId: Parent account reference for hierarchy
/// - Balance, IsControlAccount, NormalBalance, IsUsoaCompliant, RegulatoryClassification
/// - Description, Notes: Descriptive fields
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// 
/// **Validation:**
/// Validated by UpdateChartOfAccountCommandValidator to ensure:
/// - Account exists and is not locked
/// - Updated account code remains unique
/// - Parent account exists (if specified)
/// - Account type and classification are valid
/// </summary>
public record UpdateChartOfAccountCommand(
    Guid Id,
    string AccountCode,
    string AccountName,
    string AccountType,
    string UsoaCategory,
    Guid? ParentAccountId,
    string? ParentCode,
    decimal Balance,
    bool IsControlAccount,
    string NormalBalance,
    bool IsUsoaCompliant,
    string? RegulatoryClassification,
    string? Description,
    string? Notes) : ICommand<Guid>;

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
