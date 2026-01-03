namespace FSH.Modules.Microfinance.Contracts.v1.KycDocuments;

public record GetKycDocumentQuery(Guid Id);
public record GetKycDocumentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record KycDocumentsPagedResponse(List<KycDocumentSummaryDto> Items, int TotalCount, int Page, int PageSize);
