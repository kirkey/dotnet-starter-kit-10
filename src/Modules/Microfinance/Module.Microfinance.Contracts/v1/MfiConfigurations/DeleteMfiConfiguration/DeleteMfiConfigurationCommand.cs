using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.DeleteMfiConfiguration;

public sealed record DeleteMfiConfigurationCommand(Guid Id) : ICommand;
