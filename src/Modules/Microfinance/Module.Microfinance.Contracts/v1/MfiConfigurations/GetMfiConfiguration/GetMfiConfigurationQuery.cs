using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.GetMfiConfiguration;

public sealed record GetMfiConfigurationQuery(Guid Id) : IQuery<MfiConfigurationDto>;
