using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LegalActions.GetLegalAction;

public sealed record GetLegalActionQuery(Guid Id) : IQuery<LegalActionDto>;
