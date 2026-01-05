using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.DepreciationMethods.DeleteDepreciationMethod;

public sealed record DeleteDepreciationMethodCommand(Guid Id) : ICommand;
