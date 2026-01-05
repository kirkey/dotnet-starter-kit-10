using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.KycDocuments.UpdateKycDocument;

public sealed record UpdateKycDocumentCommand(Guid Id, string Name) : ICommand<Guid>;
