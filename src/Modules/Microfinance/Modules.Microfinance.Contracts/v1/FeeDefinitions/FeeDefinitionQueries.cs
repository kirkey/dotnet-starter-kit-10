namespace FSH.Modules.Microfinance.Contracts.v1.FeeDefinitions;

public record GetFeeDefinitionQuery(Guid Id);
public record GetFeeDefinitionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record FeeDefinitionsPagedResponse(List<FeeDefinitionSummaryDto> Items, int TotalCount, int Page, int PageSize);
