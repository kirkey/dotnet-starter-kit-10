using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements.GetInterconnectionAgreement;

public sealed record GetInterconnectionAgreementQuery(Guid Id) : IQuery<InterconnectionAgreementDto>;
