namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations;


public record MfiConfigurationsPagedResponse(List<MfiConfigurationSummaryDto> Items, int TotalCount, int Page, int PageSize);
