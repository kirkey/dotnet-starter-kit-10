using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.CreateCommunicationLog;

public sealed record CreateCommunicationLogCommand(string Name) : ICommand<Guid>;
