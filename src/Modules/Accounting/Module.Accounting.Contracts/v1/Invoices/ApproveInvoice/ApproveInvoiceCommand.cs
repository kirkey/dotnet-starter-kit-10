using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.ApproveInvoice;

/// <summary>
/// Approve Invoice command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to approve an Invoice, transitioning it from Draft to Approved status.
/// Approval may be required before posting to GL for audit/control requirements.
/// 
/// **Parameters:**
/// - Id: The Invoice ID to approve
/// 
/// **Business Constraints:**
/// - Invoice must be in Draft status
/// - May require approval authority (manager level)
/// - Approval gates posting to GL
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// </summary>
public record ApproveInvoiceCommand(Guid Id) : ICommand;
