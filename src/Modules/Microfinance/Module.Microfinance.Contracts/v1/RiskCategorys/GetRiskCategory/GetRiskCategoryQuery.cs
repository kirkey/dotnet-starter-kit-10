using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.RiskCategorys.GetRiskCategory;

public sealed record GetRiskCategoryQuery(Guid Id) : IQuery<RiskCategoryDto>;
