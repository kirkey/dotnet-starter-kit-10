namespace FSH.Module.Microfinance.Contracts.v1.DebtSettlements;

public record GetDebtSettlementQuery(Guid Id);
public record GetDebtSettlementsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record DebtSettlementsPagedResponse(List<DebtSettlementSummaryDto> Items, int TotalCount, int Page, int PageSize);
