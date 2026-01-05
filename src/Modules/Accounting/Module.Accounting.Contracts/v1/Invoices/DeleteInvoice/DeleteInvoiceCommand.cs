using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.DeleteInvoice;

/// <summary>
/// Delete Invoice command DTO.
/// 
/// **Purpose:**
/// Encapsulates the request to delete an Invoice (only in Draft state).
/// Posted or Paid invoices cannot be deleted; they must be unapproved/reversed instead.
/// 
/// **Parameters:**
/// - Id: The unique identifier of the Invoice to delete
/// 
/// **Multi-Tenancy:**
/// Tenant context is automatically applied via DbContext filters.
/// 
/// **Validation:**
/// The handler validates:
/// - Invoice exists in the current tenant
/// - Invoice status is Draft (cannot delete Posted or Paid invoices)
/// </summary>
public record DeleteInvoiceCommand(Guid Id) : ICommand;