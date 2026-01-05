using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.BudgetDetails.DeleteBudgetDetail;

public sealed record DeleteBudgetDetailCommand(Guid Id) : ICommand;