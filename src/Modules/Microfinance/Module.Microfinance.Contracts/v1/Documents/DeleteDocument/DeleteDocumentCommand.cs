using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Documents.DeleteDocument;

public sealed record DeleteDocumentCommand(Guid Id) : ICommand;
