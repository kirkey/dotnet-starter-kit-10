using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.RetainedEarnings.ReopenRetainedEarnings;

public sealed record ReopenRetainedEarningsCommand(Guid Id, string? Reason = null) : ICommand;