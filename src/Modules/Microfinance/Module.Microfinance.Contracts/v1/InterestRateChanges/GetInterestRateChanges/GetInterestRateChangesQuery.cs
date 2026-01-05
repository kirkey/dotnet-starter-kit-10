using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.GetInterestRateChanges;

public sealed record GetInterestRateChangesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<InterestRateChangesPagedResponse>;

public sealed record InterestRateChangesPagedResponse(List<InterestRateChangeSummaryDto> Items, int TotalCount, int Page, int PageSize);
