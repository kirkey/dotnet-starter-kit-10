namespace FSH.Modules.Microfinance.Contracts.v1.FeeWaivers;

public record GetFeeWaiverQuery(Guid Id);
public record GetFeeWaiversQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record FeeWaiversPagedResponse(List<FeeWaiverSummaryDto> Items, int TotalCount, int Page, int PageSize);
