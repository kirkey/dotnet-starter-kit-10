using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.GetChartOfAccount;

/// <summary>
/// Get Chart of Account query.
/// 
/// **Purpose:**
/// Encapsulates the request to retrieve a specific Chart of Account by its ID.
/// 
/// **Parameters:**
/// - Id: The unique identifier of the Chart of Account to retrieve
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via query filters.
/// 
/// **Validation:**
/// The handler validates:
/// - Account exists in the current tenant
/// - Account ID is valid and not deleted
/// </summary>
public record GetChartOfAccountQuery(Guid Id) : IQuery<ChartOfAccountDto>;
