namespace FSH.Modules.Microfinance.Contracts.v1.FeeCharges;

public record GetFeeChargeQuery(Guid Id);
public record GetFeeChargesQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record FeeChargesPagedResponse(List<FeeChargeSummaryDto> Items, int TotalCount, int Page, int PageSize);
