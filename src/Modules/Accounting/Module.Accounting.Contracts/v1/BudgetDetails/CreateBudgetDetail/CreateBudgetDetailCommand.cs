using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BudgetDetails.CreateBudgetDetail;

public sealed record CreateBudgetDetailCommand(string Name, string? Description = null) : ICommand<Guid>;