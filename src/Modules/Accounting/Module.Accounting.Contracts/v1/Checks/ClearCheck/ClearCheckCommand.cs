using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.ClearCheck;

/// <summary>
/// Clear Check command to transition a check to cleared/reconciled status.
/// </summary>
/// <param name="Id">Check ID to clear (must exist in issued status)</param>
public record ClearCheckCommand(Guid Id) : ICommand;
