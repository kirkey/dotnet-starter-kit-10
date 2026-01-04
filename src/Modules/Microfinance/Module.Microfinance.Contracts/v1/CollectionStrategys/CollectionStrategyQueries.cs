namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys;

public record GetCollectionStrategyQuery(Guid Id);
public record GetCollectionStrategysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollectionStrategysPagedResponse(List<CollectionStrategySummaryDto> Items, int TotalCount, int Page, int PageSize);
