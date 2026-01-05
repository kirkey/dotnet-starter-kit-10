using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.DeleteInterconnectionAgreement;

public sealed record DeleteInterconnectionAgreementCommand(Guid Id) : ICommand;
