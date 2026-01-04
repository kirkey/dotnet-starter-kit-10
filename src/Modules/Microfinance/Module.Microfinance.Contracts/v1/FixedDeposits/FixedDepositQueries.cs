namespace FSH.Module.Microfinance.Contracts.v1.FixedDeposits;

public record GetFixedDepositQuery(Guid Id);
public record GetFixedDepositsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record FixedDepositsPagedResponse(List<FixedDepositSummaryDto> Items, int TotalCount, int Page, int PageSize);
