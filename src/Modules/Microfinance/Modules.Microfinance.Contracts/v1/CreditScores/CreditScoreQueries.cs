namespace FSH.Modules.Microfinance.Contracts.v1.CreditScores;

public record GetCreditScoreQuery(Guid Id);
public record GetCreditScoresQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record CreditScoresPagedResponse(List<CreditScoreSummaryDto> Items, int TotalCount, int Page, int PageSize);
