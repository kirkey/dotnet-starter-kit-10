using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.SendInvoice;

/// <summary>
/// Send Invoice command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to send an Invoice to the customer/vendor.
/// Marks invoice as sent for tracking delivery and payment deadline purposes.
/// 
/// **Parameters:**
/// - Id: The Invoice ID to send
/// 
/// **Business Constraints:**
/// - Invoice must be in Approved status (or Draft, depending on workflow)
/// - May trigger email notification to customer/vendor
/// - Records sent date for payment tracking
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
public record SendInvoiceCommand(Guid Id) : ICommand;