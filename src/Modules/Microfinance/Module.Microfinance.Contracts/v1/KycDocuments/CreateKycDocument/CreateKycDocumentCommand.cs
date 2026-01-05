using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.KycDocuments.CreateKycDocument;

public sealed record CreateKycDocumentCommand(string Name) : ICommand<Guid>;
