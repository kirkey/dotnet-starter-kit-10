using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.DeleteFeeDefinition;

public sealed record DeleteFeeDefinitionCommand(Guid Id) : ICommand;
