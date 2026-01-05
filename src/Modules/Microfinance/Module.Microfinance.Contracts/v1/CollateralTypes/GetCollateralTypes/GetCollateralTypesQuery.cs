using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollateralTypes.GetCollateralTypes;

public sealed record GetCollateralTypesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollateralTypesPagedResponse>;
