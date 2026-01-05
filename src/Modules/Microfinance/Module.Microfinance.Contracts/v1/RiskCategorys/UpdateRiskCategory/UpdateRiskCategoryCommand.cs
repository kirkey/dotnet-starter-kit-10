using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskCategorys.UpdateRiskCategory;

public sealed record UpdateRiskCategoryCommand(Guid Id, string Name) : ICommand<Guid>;
