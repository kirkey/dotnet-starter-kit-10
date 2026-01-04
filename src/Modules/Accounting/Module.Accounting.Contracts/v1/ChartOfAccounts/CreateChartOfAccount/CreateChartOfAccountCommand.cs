using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.CreateChartOfAccount;

/// <summary>
/// Create Chart of Account command.
/// 
/// **Purpose:**
/// Encapsulates the request to create a new account in the chart of accounts.
/// Includes all properties needed to establish the account in the general ledger.
/// 
/// **Multi-Tenancy:**
/// Tenant is automatically assigned from the current user context.
/// 
/// **Validation:**
/// Validated by CreateChartOfAccountCommandValidator to ensure:
/// - Account code is unique within the chart of accounts
/// - Account name is not empty
/// - Account type is valid
/// - Balance is appropriate for account type
/// </summary>
public record CreateChartOfAccountCommand(
    string AccountCode,
    string AccountName,
    string AccountType,
    string UsoaCategory,
    Guid? ParentAccountId = null,
    string? ParentCode = null,
    decimal Balance = 0,
    bool IsControlAccount = false,
    string NormalBalance = "Debit",
    bool IsUsoaCompliant = true,
    string? RegulatoryClassification = null,
    string? Description = null,
    string? Notes = null) : ICommand<Guid>;
