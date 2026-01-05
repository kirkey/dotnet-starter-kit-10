using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AmlAlerts.UpdateAmlAlert;

public sealed record UpdateAmlAlertCommand(Guid Id, string Name) : ICommand<Guid>;
