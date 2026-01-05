using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings.UpdateAgentBanking;

public sealed record UpdateAgentBankingCommand(Guid Id, string Name) : ICommand<Guid>;
