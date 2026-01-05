using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Documents.GetDocument;

public sealed record GetDocumentQuery(Guid Id) : IQuery<DocumentDto>;
