using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.CreatePrepaidExpense;

public sealed record CreatePrepaidExpenseCommand(string Name, string? Description) : ICommand<Guid>;
