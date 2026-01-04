using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.DeleteCheck;

/// <summary>
/// Delete Check command to delete an existing check from the system.
/// </summary>
/// <param name="Id">Check ID to delete (must exist, preferably Draft status)</param>
public record DeleteCheckCommand(Guid Id) : ICommand;
