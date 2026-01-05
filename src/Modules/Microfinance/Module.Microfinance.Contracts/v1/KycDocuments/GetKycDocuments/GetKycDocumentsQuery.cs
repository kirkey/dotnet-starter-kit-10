using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.KycDocuments.GetKycDocuments;

public sealed record GetKycDocumentsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<KycDocumentsPagedResponse>;
