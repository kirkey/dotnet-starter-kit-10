namespace Accounting.Application.AccountingPeriods.Create.v1;

/// <summary>
/// Command to create a new accounting period.
/// </summary>
/// <param name>Human-friendly name for the period (required, max length enforced by domain).</param>
/// <param name>Inclusive start date of the period.</param>
/// <param name>Inclusive end date of the period; must be after <paramref name/>.</param>
/// <param name>Fiscal year the period belongs to (e.g. 2025).</param>
/// <param name>Period granularity, e.g. "Monthly", "Quarterly", "Yearly".</param>
/// <param name>True when this period is an adjustment period (e.g. period 13).</param>
/// <param name>Optional long description.</param>
/// <param name>Optional admin notes.</param>
public sealed record CreateAccountingPeriodCommand(
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int FiscalYear,
    string PeriodType,
    bool IsAdjustmentPeriod,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;
