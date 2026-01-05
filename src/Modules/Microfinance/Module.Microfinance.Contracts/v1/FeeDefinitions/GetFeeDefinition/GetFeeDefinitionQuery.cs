using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.GetFeeDefinition;

public sealed record GetFeeDefinitionQuery(Guid Id) : IQuery<FeeDefinitionDto>;
