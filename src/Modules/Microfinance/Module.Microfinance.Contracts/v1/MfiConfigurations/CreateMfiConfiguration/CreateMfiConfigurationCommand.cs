using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.CreateMfiConfiguration;

public sealed record CreateMfiConfigurationCommand(string Name) : ICommand<Guid>;
