using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Budgets.ApproveBudget;

/// <summary>
/// Approve Budget command.
/// </summary>
public record ApproveBudgetCommand(Guid Id) : ICommand;
