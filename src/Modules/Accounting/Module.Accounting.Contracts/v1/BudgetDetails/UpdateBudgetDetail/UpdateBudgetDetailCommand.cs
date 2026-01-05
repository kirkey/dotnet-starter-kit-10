using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BudgetDetails.UpdateBudgetDetail;

public sealed record UpdateBudgetDetailCommand(Guid Id, string Name, string? Description = null) : ICommand<Guid>;