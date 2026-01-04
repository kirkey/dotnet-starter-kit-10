namespace FSH.Module.Microfinance.Contracts.v1.Documents;

public record GetDocumentQuery(Guid Id);
public record GetDocumentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record DocumentsPagedResponse(List<DocumentSummaryDto> Items, int TotalCount, int Page, int PageSize);
