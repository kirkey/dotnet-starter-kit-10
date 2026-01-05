using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskCategorys.DeleteRiskCategory;

public sealed record DeleteRiskCategoryCommand(Guid Id) : ICommand;
