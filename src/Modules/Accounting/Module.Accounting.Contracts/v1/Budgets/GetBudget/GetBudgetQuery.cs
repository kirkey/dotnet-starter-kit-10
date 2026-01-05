using FSH.Module.Accounting.Contracts.v1.Budgets;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Budgets.GetBudget;

/// <summary>
/// Get Budget query.
/// </summary>
public record GetBudgetQuery(Guid Id) : IQuery<BudgetDto>;