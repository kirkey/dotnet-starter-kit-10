namespace Accounting.Application.PrepaidExpenses.Create.v1;

/// <summary>
/// Command to create a new prepaid expense.
/// </summary>
public record PrepaidExpenseCreateCommand(
    string PrepaidNumber,
    string Description,
    decimal TotalAmount,
    DateTime StartDate,
    DateTime EndDate,
    DefaultIdType PrepaidAssetAccountId,
    DefaultIdType ExpenseAccountId,
    DateTime PaymentDate,
    string AmortizationSchedule,
    DefaultIdType? VendorId,
    string? VendorName,
    DefaultIdType? PaymentId,
    DefaultIdType? CostCenterId,
    DefaultIdType? PeriodId,
    string? Notes
) : IRequest<PrepaidExpenseCreateResponse>;

