using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.AmlAlerts.DeleteAmlAlert;

public sealed record DeleteAmlAlertCommand(Guid Id) : ICommand;
