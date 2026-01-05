using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.UpdateInterconnectionAgreement;

public sealed record UpdateInterconnectionAgreementCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;
