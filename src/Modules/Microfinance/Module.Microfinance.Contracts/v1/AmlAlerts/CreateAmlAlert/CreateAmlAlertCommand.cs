using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AmlAlerts.CreateAmlAlert;

public sealed record CreateAmlAlertCommand(string Name) : ICommand<Guid>;
