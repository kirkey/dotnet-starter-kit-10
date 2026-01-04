namespace Accounting.Application.Budgets.Create;

/// <summary>
/// Command to create a new Budget aggregate.
/// </summary>
/// <param name>Human-friendly budget name. Required, max 256.</param>
/// <param name>Accounting period identifier. Required.</param>
/// <param name>Accounting period human-readable name. Required, max 128.</param>
/// <param name>Fiscal year (1900-2100). Required.</param>
/// <param name>Budget type (e.g., Operating, Capital, Cash Flow). Required, max 32.</param>
/// <param name>Optional description, max 1000.</param>
/// <param name>Optional notes, max 1000.</param>
public sealed record CreateBudgetCommand(
    string Name,
    DefaultIdType PeriodId,
    string PeriodName,
    int FiscalYear,
    string BudgetType,
    string? Description,
    string? Notes
) : IRequest<CreateBudgetResponse>;
