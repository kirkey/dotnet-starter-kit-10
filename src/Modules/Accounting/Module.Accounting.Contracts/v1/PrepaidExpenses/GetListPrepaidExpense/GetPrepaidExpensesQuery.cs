using Mediator;
using FSH.Module.Accounting.Contracts.v1.PrepaidExpenses;

namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.GetListPrepaidExpense;

public sealed record GetPrepaidExpensesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PrepaidExpensesPagedResponse>;

public sealed record PrepaidExpensesPagedResponse(
    List<PrepaidExpenseSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);
