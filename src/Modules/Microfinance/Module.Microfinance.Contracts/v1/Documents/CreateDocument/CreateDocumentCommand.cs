using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Documents.CreateDocument;

public sealed record CreateDocumentCommand(string Name) : ICommand<Guid>;
