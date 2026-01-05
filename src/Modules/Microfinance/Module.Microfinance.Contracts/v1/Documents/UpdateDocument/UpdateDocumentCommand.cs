using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Documents.UpdateDocument;

public sealed record UpdateDocumentCommand(Guid Id, string Name) : ICommand<Guid>;
