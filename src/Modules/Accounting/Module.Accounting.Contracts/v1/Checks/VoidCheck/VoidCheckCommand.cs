using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.VoidCheck;

/// <summary>
/// Void Check command to cancel and void an existing check.
/// </summary>
/// <param name="Id">Check ID to void (must exist)</param>
public record VoidCheckCommand(Guid Id) : ICommand;