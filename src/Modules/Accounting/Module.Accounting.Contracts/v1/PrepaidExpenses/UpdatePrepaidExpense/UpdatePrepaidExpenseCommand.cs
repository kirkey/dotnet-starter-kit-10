using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.UpdatePrepaidExpense;

public sealed record UpdatePrepaidExpenseCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;
