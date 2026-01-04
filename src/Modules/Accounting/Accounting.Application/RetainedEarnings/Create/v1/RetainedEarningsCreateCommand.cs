namespace Accounting.Application.RetainedEarnings.Create.v1;

/// <summary>
/// Command to create a new retained earnings record for a fiscal year.
/// </summary>
public record RetainedEarningsCreateCommand(
    int FiscalYear,
    decimal OpeningBalance,
    DateTime FiscalYearStartDate,
    DateTime FiscalYearEndDate,
    DefaultIdType? RetainedEarningsAccountId,
    string? Description,
    string? Notes
) : IRequest<RetainedEarningsCreateResponse>;

