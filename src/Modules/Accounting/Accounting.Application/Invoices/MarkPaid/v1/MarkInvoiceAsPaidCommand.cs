namespace Accounting.Application.Invoices.MarkPaid.v1;

/// <summary>
/// Command to mark an invoice as fully paid.
/// </summary>
/// <param name>Invoice identifier.</param>
/// <param name>Date when payment was received.</param>
/// <param name>Method used for payment (e.g., Cash, Check, Credit Card).</param>
public sealed record MarkInvoiceAsPaidCommand(
    DefaultIdType InvoiceId,
    DateTime PaidDate,
    string? PaymentMethod
) : IRequest<MarkInvoiceAsPaidResponse>;

