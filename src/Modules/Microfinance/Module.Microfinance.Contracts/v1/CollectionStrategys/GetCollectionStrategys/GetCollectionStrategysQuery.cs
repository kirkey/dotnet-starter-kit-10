using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CollectionStrategys.GetCollectionStrategys;

public sealed record GetCollectionStrategysQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<CollectionStrategysPagedResponse>;

public sealed record CollectionStrategysPagedResponse(List<CollectionStrategySummaryDto> Items, int TotalCount, int Page, int PageSize);
