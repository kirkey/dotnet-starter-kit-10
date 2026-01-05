using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.CreateFeeDefinition;

public sealed record CreateFeeDefinitionCommand(string Name) : ICommand<Guid>;
