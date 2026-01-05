using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.KycDocuments.DeleteKycDocument;

public sealed record DeleteKycDocumentCommand(Guid Id) : ICommand;
