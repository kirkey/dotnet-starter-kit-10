using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CreateRetainedEarnings;

public sealed record CreateRetainedEarningsCommand(string Name, string? Description = null) : ICommand<Guid>;