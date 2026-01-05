using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.KycDocuments.GetKycDocument;

public sealed record GetKycDocumentQuery(Guid Id) : IQuery<KycDocumentDto>;
