namespace FSH.Module.Microfinance.Contracts.v1.CollectionCases;

public record GetCollectionCaseQuery(Guid Id);
public record GetCollectionCasesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollectionCasesPagedResponse(List<CollectionCaseSummaryDto> Items, int TotalCount, int Page, int PageSize);
