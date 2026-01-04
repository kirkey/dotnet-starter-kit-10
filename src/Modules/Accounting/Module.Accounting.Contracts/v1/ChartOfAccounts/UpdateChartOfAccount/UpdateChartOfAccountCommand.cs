using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.UpdateChartOfAccount;

/// <summary>
/// Update Chart of Account command.
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
