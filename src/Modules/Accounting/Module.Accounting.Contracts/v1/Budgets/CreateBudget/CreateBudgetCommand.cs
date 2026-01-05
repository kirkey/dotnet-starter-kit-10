using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Budgets.CreateBudget;

/// <summary>
/// Create Budget command.
/// </summary>
public record CreateBudgetCommand(string Name, string? Description) : ICommand<Guid>;