using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DepreciationMethods.UpdateDepreciationMethod;

public sealed record UpdateDepreciationMethodCommand(Guid Id, string Name, string? Description) : ICommand<Guid>;
