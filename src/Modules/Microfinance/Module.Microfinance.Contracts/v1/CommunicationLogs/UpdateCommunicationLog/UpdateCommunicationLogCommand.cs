using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.UpdateCommunicationLog;

public sealed record UpdateCommunicationLogCommand(Guid Id, string Name) : ICommand<Guid>;
