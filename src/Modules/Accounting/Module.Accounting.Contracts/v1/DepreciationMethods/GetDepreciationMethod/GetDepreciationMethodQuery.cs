using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DepreciationMethods.GetDepreciationMethod;

public sealed record GetDepreciationMethodQuery(Guid Id) : IQuery<DepreciationMethodDto>;
