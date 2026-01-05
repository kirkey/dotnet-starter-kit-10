using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.GetPrepaidExpense;

public sealed record GetPrepaidExpenseQuery(Guid Id) : IQuery<PrepaidExpenseDto>;
