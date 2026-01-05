using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings.GetAgentBanking;

public sealed record GetAgentBankingQuery(Guid Id) : IQuery<AgentBankingDto>;
