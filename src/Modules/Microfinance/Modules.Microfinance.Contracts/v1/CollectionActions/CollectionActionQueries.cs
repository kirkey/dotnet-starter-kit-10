namespace FSH.Modules.Microfinance.Contracts.v1.CollectionActions;

public record GetCollectionActionQuery(Guid Id);
public record GetCollectionActionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CollectionActionsPagedResponse(List<CollectionActionSummaryDto> Items, int TotalCount, int Page, int PageSize);
