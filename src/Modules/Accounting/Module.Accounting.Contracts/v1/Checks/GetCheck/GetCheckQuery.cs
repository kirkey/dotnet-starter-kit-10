using FSH.Module.Accounting.Contracts.v1.Checks;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.GetCheck;

/// <summary>
/// Get Check query to retrieve a single check by ID.
/// </summary>
/// <param name="Id">Check ID (Guid) to retrieve</param>
public record GetCheckQuery(Guid Id) : IQuery<CheckDto>;