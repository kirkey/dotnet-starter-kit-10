using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.DeleteChartOfAccount;

/// <summary>
/// Delete Chart of Account command.
/// 
/// **Purpose:**
/// Encapsulates the request to delete a Chart of Account from the general ledger.
/// 
/// **Parameters:**
/// - Id: The unique identifier of the Chart of Account to delete
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// 
/// **Validation:**
/// The handler validates:
/// - Account exists in the current tenant
/// - Account is not in use (no posted entries)
/// - Account is not a parent account with children
/// - Account is not locked or archived
/// </summary>
public record DeleteChartOfAccountCommand(Guid Id) : ICommand;
