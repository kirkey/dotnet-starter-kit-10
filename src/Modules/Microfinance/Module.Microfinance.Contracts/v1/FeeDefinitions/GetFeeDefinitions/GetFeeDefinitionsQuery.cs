using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.GetFeeDefinitions;

public sealed record GetFeeDefinitionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<FeeDefinitionsPagedResponse>;

public sealed record FeeDefinitionsPagedResponse(List<FeeDefinitionSummaryDto> Items, int TotalCount, int Page, int PageSize);
