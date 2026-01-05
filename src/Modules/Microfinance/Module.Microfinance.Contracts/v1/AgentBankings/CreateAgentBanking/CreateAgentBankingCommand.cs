using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings.CreateAgentBanking;

public sealed record CreateAgentBankingCommand(string Name) : ICommand<Guid>;
