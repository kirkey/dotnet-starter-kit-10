using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.PrepaidExpenses.AmortizePrepaidExpense;

public sealed record AmortizePrepaidExpenseCommand(Guid Id) : ICommand;
