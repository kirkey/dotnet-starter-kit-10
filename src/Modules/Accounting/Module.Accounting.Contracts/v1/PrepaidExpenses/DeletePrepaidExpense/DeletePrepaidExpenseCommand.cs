using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.DeletePrepaidExpense;

public sealed record DeletePrepaidExpenseCommand(Guid Id) : ICommand;
