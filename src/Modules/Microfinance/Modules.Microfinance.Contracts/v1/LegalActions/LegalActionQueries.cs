namespace FSH.Modules.Microfinance.Contracts.v1.LegalActions;

public record GetLegalActionQuery(Guid Id);
public record GetLegalActionsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record LegalActionsPagedResponse(List<LegalActionSummaryDto> Items, int TotalCount, int Page, int PageSize);
