using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AgentBankings.DeleteAgentBanking;

public sealed record DeleteAgentBankingCommand(Guid Id) : ICommand;
