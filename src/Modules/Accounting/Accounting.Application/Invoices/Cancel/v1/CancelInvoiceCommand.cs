namespace Accounting.Application.Invoices.Cancel.v1;

/// <summary>
/// Command to cancel an invoice.
/// </summary>
/// <param name>Invoice identifier.</param>
/// <param name>Reason for cancellation.</param>
public sealed record CancelInvoiceCommand(
    DefaultIdType InvoiceId,
    string? Reason
) : IRequest<CancelInvoiceResponse>;

