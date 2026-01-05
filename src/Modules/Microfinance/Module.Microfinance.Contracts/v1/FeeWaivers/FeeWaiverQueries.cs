namespace FSH.Module.Microfinance.Contracts.v1.FeeWaivers;

public record FeeWaiversPagedResponse(List<FeeWaiverSummaryDto> Items, int TotalCount, int Page, int PageSize);
