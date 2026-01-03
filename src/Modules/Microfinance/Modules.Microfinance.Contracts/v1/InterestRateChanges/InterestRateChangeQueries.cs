namespace FSH.Modules.Microfinance.Contracts.v1.InterestRateChanges;

public record GetInterestRateChangeQuery(Guid Id);
public record GetInterestRateChangesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record InterestRateChangesPagedResponse(List<InterestRateChangeSummaryDto> Items, int TotalCount, int Page, int PageSize);
