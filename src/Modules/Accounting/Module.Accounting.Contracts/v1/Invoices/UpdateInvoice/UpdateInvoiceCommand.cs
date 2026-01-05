using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.UpdateInvoice;

/// <summary>
/// Update Invoice command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to update an existing Invoice's metadata before posting.
/// Note: Line items must be managed separately (delete/recreate pattern).
/// 
/// **Parameters:**
/// - Id: The Invoice ID to update
/// - InvoiceNumber, InvoiceDate, InvoiceType, DueDate
/// - BillToName, CustomerId, VendorId
/// - Address fields: BillToAddress, ShipToName, ShipToAddress
/// - PaymentTerms, TaxCode, CurrencyCode, ExchangeRate
/// - ReferenceNumber, PurchaseOrderNumber, Description, Notes, Terms
/// 
/// **Constraints:**
/// - Can only be updated if Status is Draft
/// - Cannot change line items via update (delete/recreate instead)
/// - Cannot change posted status via this command
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
public record UpdateInvoiceCommand(
    Guid Id,
    string InvoiceNumber,
    DateTime InvoiceDate,
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