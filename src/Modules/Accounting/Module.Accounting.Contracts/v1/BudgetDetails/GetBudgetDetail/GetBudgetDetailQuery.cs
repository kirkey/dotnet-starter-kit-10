using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BudgetDetails.GetBudgetDetail;

public sealed record GetBudgetDetailQuery(Guid Id) : IQuery<BudgetDetailDto>;