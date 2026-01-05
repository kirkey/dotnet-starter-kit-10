using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.UpdateMfiConfiguration;

public sealed record UpdateMfiConfigurationCommand(Guid Id, string Name) : ICommand<Guid>;
