using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Documents.GetDocuments;

public sealed record GetDocumentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<DocumentsPagedResponse>;

public sealed record DocumentsPagedResponse(List<DocumentSummaryDto> Items, int TotalCount, int Page, int PageSize);
