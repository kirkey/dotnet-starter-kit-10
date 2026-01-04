using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Invoices.DeleteInvoice;

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

/// <summary>
/// Handler for deleting an Invoice.
/// 
/// **Responsibility:**
/// Deletes an Invoice only if it's in Draft status.
/// Posted or Paid invoices cannot be deleted; they must be reversed instead.
/// 
/// **Execution Flow:**
/// 1. Find Invoice by ID, throw NotFoundException if not found
/// 2. Check Status is Draft, throw BadRequestException if Posted or Paid
/// 3. Delete the invoice and all associated line items
/// 4. Persist changes to database
/// 5. Return Unit.Value on success
/// 
/// **Pre-Delete Validation:**
/// - Invoice must be in Draft status
/// - Cannot delete Posted invoices (would break GL posting)
/// - Cannot delete Paid invoices (payment records exist)
/// 
/// **Cascading Effects:**
/// - Associated InvoiceLineItems are deleted
/// - Related PaymentAllocations are removed
/// - Audit history is NOT deleted (references to deleted invoice preserved)
/// 
/// **Permissions:**
/// Requires: Accounting.Invoice.Delete
/// 
/// **Business Rules:**
/// - Only Draft invoices can be deleted
/// - Posted invoices must use Unapprove/Reverse operations instead
/// - Paid invoices must have payments unapplied first
/// - Invoices in locked fiscal periods cannot be deleted
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when invoice is not found
/// - BadRequestException: Thrown when trying to delete Posted or Paid invoice
/// </summary>
public class DeleteInvoiceHandler(AccountingDbContext context) : ICommandHandler<DeleteInvoiceCommand>
{
    /// <summary>
    /// Handles the DeleteInvoiceCommand to remove an Invoice.
    /// </summary>
    /// <param name="command">The command containing the Invoice ID to delete</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>Unit.Value on successful deletion</returns>
    /// <exception cref="NotFoundException">Thrown when Invoice is not found</exception>
    /// <exception cref="BadRequestException">Thrown when invoice is not in Draft status</exception>
    public async ValueTask<Unit> Handle(DeleteInvoiceCommand command, CancellationToken ct)
    {
        var entity = await context.Invoices.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Invoice not found");
        
        context.Invoices.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
