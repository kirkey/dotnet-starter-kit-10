using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.GetCommunicationLog;

public sealed record GetCommunicationLogQuery(Guid Id) : IQuery<CommunicationLogDto>;
