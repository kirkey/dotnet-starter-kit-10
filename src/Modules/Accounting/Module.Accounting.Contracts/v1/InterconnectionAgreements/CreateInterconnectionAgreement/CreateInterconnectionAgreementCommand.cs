using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.CreateInterconnectionAgreement;

public sealed record CreateInterconnectionAgreementCommand(string Name, string? Description) : ICommand<Guid>;
