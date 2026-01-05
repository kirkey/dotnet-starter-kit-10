using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.UpdateFeeDefinition;

public sealed record UpdateFeeDefinitionCommand(Guid Id, string Name) : ICommand<Guid>;
