using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Budgets.UpdateBudget;

/// <summary>
/// Update Budget command.
/// </summary>
public record UpdateBudgetCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;