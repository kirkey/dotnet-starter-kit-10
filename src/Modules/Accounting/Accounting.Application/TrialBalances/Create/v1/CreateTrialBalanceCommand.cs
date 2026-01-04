namespace Accounting.Application.TrialBalances.Create.v1;

/// <summary>
/// Command to create a new trial balance report.
/// </summary>
/// <remarks>
/// Trial Balance Creation:
/// - TrialBalanceNumber: Unique identifier (e.g., "TB-2025-10")
/// - PeriodId: Accounting period reference
/// - PeriodStartDate/PeriodEndDate: Period date range
/// - IncludeZeroBalances: Whether to include accounts with zero balances
/// 
/// Business Rules:
/// - Trial balance is created in Draft status
/// - Line items can be added after creation
/// - Must be balanced before finalizing
/// - Auto-generates from GL if requested
/// </remarks>
public sealed record CreateTrialBalanceCommand(
    string TrialBalanceNumber,
    DefaultIdType PeriodId,
    DateTime PeriodStartDate,
    DateTime PeriodEndDate,
    bool IncludeZeroBalances,
    bool AutoGenerate,
    string? Description,
    string? Notes
) : IRequest<TrialBalanceCreateResponse>;
