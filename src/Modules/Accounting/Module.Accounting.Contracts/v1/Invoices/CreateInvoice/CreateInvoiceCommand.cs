using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.CreateInvoice;

/// <summary>
/// Create Invoice command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to create a new invoice for either sales (customer) or purchases (vendor).
/// Supports multi-currency transactions with exchange rates.
/// 
/// **Parameters:**
/// - InvoiceNumber: Unique invoice identifier (must be unique per type)
/// - InvoiceDate: Date the invoice was issued
/// - InvoiceType: Type of invoice (Sales Invoice, Purchase Invoice, Credit Memo, Debit Memo, etc.)
/// - DueDate: Payment due date for the invoice
/// - BillToName: Name of the billing party
/// - CustomerId: Customer reference (for sales invoices)
/// - VendorId: Vendor reference (for purchase invoices)
/// - BillToAddress, ShipToName, ShipToAddress: Address information
/// - PaymentTerms: Terms code (Net 30, Net 60, Due on Receipt, etc.)
/// - TaxCode: Tax classification code
/// - CurrencyCode: ISO 4217 currency code (default USD)
/// - ExchangeRate: Exchange rate to functional currency (default 1.0)
/// - ReferenceNumber: External reference (PO number, order number, etc.)
/// - PurchaseOrderNumber: Associated PO number
/// - Description, Notes, Terms: Descriptive fields
/// 
/// **Multi-Tenancy:**
/// Tenant is automatically assigned from the current user context.
/// 
/// **Validation:**
/// Validated by CreateInvoiceCommandValidator to ensure:
/// - InvoiceNumber is unique within the invoice type
/// - CustomerId OR VendorId is provided (not both)
/// - InvoiceDate and DueDate are valid
/// - ExchangeRate is positive
/// </summary>
public record CreateInvoiceCommand(
    string InvoiceNumber,
    DateTime InvoiceDate,
    string InvoiceType,
    DateTime DueDate,
    string BillToName,
    Guid? CustomerId = null,
    Guid? VendorId = null,
    string? BillToAddress = null,
    string? ShipToName = null,
    string? ShipToAddress = null,
    string? PaymentTerms = null,
    string? TaxCode = null,
    string? CurrencyCode = null,
    decimal ExchangeRate = 1.0m,
    string? ReferenceNumber = null,
    string? PurchaseOrderNumber = null,
    string? Description = null,
    string? Notes = null,
    string? Terms = null) : ICommand<Guid>;