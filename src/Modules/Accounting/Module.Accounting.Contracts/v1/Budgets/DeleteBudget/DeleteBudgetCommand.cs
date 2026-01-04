using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Budgets.DeleteBudget;

/// <summary>
/// Delete Budget command.
/// </summary>
public record DeleteBudgetCommand(Guid Id) : ICommand;
