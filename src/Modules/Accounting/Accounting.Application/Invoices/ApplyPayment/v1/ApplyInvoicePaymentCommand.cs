namespace Accounting.Application.Invoices.ApplyPayment.v1;

/// <summary>
/// Command to apply a partial payment to an invoice.
/// </summary>
/// <param name>Invoice identifier.</param>
/// <param name>Payment amount to apply.</param>
/// <param name>Date when payment was received.</param>
/// <param name>Method used for payment (e.g., Cash, Check, Credit Card).</param>
public sealed record ApplyInvoicePaymentCommand(
    DefaultIdType InvoiceId,
    decimal Amount,
    DateTime PaymentDate,
    string? PaymentMethod
) : IRequest<ApplyInvoicePaymentResponse>;

