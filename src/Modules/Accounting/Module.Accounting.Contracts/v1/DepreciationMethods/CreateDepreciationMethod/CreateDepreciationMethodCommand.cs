using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DepreciationMethods.CreateDepreciationMethod;

public sealed record CreateDepreciationMethodCommand(string Name, string? Description) : ICommand<Guid>;
