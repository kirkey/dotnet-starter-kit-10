using FSH.Module.Accounting.Contracts.v1.Invoices;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.GetInvoice;

/// <summary>
/// Get Invoice query DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to retrieve a specific Invoice by its ID.
/// Includes all invoice metadata, amounts, and posting/payment status.
/// 
/// **Parameters:**
/// - Id: The unique identifier of the Invoice to retrieve
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via query filters.
/// 
/// **Returned Data:**
/// Complete invoice with all fields including customer/vendor, amounts, status
/// </summary>
public record GetInvoiceQuery(Guid Id) : IQuery<InvoiceDto>;