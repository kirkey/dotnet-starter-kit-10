using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskCategorys.CreateRiskCategory;

public sealed record CreateRiskCategoryCommand(string Name) : ICommand<Guid>;
