using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.DeleteCommunicationLog;

public sealed record DeleteCommunicationLogCommand(Guid Id) : ICommand;
