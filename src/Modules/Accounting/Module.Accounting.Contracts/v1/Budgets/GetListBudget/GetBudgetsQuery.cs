using FSH.Module.Accounting.Contracts.v1.Budgets;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Budgets.GetListBudget;

/// <summary>
/// Get Budgets (paginated) query.
/// </summary>
public record GetBudgetsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<BudgetsPagedResponse>;

/// <summary>
/// Paginated response for budgets listing.
/// </summary>
public record BudgetsPagedResponse(
    List<BudgetSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for budget list items.
/// </summary>
