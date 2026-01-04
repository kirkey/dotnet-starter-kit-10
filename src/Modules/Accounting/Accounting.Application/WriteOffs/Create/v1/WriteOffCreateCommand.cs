namespace Accounting.Application.WriteOffs.Create.v1;

/// <summary>
/// Command to create a new write-off.
/// </summary>
public record WriteOffCreateCommand(
    string ReferenceNumber,
    DateTime WriteOffDate,
    string WriteOffType,
    decimal Amount,
    DefaultIdType ReceivableAccountId,
    DefaultIdType ExpenseAccountId,
    DefaultIdType? CustomerId,
    string? CustomerName,
    DefaultIdType? InvoiceId,
    string? InvoiceNumber,
    string? Reason,
    string? Description,
    string? Notes
) : IRequest<WriteOffCreateResponse>;

