using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.IssueCheck;

/// <summary>
/// Issue Check command to transition a check from Draft/Printed status to Issued status.
/// </summary>
/// <param name="Id">Check ID to issue (must exist in Draft or Printed status)</param>
public record IssueCheckCommand(Guid Id) : ICommand;