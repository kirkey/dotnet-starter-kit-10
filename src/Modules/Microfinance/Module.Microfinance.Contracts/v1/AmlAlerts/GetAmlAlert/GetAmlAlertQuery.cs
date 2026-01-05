using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AmlAlerts.GetAmlAlert;

public sealed record GetAmlAlertQuery(Guid Id) : IQuery<AmlAlertDto>;
