namespace FSH.Modules.Microfinance.Contracts.v1.MfiConfigurations;

public record GetMfiConfigurationQuery(Guid Id);
public record GetMfiConfigurationsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record MfiConfigurationsPagedResponse(List<MfiConfigurationSummaryDto> Items, int TotalCount, int Page, int PageSize);
