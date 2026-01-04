using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.StopPaymentCheck;

/// <summary>
/// Stop Payment Check command to prevent a check from being cleared/cashed.
/// </summary>
/// <param name="Id">Check ID to stop payment (must exist in issued status)</param>
public record StopPaymentCheckCommand(Guid Id) : ICommand;
