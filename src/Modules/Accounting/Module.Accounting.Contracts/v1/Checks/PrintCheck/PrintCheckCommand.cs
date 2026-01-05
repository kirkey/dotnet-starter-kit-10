using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.PrintCheck;

/// <summary>
/// Print Check command to transition a check from Draft to Printed status.
/// </summary>
/// <param name="Id">Check ID to print (must exist in Draft status)</param>
public record PrintCheckCommand(Guid Id) : ICommand;