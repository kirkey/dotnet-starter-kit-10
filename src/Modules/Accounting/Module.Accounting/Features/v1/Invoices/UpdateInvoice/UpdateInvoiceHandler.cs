using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Invoices.UpdateInvoice;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Invoices.UpdateInvoice;

/// <summary>
/// Handler for updating an Invoice.
/// 
/// **Responsibility:**
/// Updates Invoice metadata (before posting). Cannot modify line items or amounts.
/// 
/// **Execution Flow:**
/// 1. Find Invoice by ID, throw NotFoundException if not found
/// 2. Verify invoice is in Draft status (not Posted or Paid)
/// 3. Call entity.Update() domain method with new metadata
/// 4. Persist changes to database
/// 5. Return updated invoice ID
/// 
/// **Updateable Fields:**
/// - InvoiceNumber, InvoiceDate, InvoiceType
/// - DueDate, BillToName, CustomerId, VendorId
/// - Address fields (BillToAddress, ShipToName, ShipToAddress)
/// - PaymentTerms, TaxCode, CurrencyCode, ExchangeRate
/// - ReferenceNumber, PurchaseOrderNumber, Description, Notes, Terms
/// 
/// **Immutable/Protected Fields:**
/// - Id: Cannot change
/// - Line items: Cannot be updated (must delete/recreate)
/// - Amounts (SubTotal, Tax, Discount, Shipping, Total): Calculated from line items
/// - IsPosted, IsPaid: Set via separate operations
/// - PostedDate, PaidDate: Set on posting/payment
/// - JournalEntryId: Set on posting
/// 
/// **Permissions:**
/// Requires: Accounting.Invoice.Edit
/// 
/// **Business Rules:**
/// - Can only update Draft invoices
/// - Cannot update Posted invoices (must unpost first)
/// - Cannot update Paid invoices
/// - Cannot update invoices in locked fiscal periods
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when invoice not found
/// - BadRequestException: Thrown if invoice is not in Draft status
/// </summary>
public class UpdateInvoiceHandler(AccountingDbContext context) : ICommandHandler<UpdateInvoiceCommand, Guid>
{
    /// <summary>
    /// Handles the UpdateInvoiceCommand to update invoice metadata.
    /// </summary>
    /// <param name="command">The command containing the Invoice updates</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ID of the updated Invoice</returns>
    /// <exception cref="NotFoundException">Thrown when Invoice is not found</exception>
    public async ValueTask<Guid> Handle(UpdateInvoiceCommand command, CancellationToken ct)
    {
        var entity = await context.Invoices.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Invoice not found");
        
        entity.Update(
            invoiceNumber: command.InvoiceNumber,
            invoiceDate: command.InvoiceDate,
            dueDate: command.DueDate,
            billToName: command.BillToName,
            billToAddress: command.BillToAddress,
            shipToName: command.ShipToName,
            shipToAddress: command.ShipToAddress,
            paymentTerms: command.PaymentTerms,
            taxCode: command.TaxCode,
            currencyCode: command.CurrencyCode,
            exchangeRate: command.ExchangeRate,
            referenceNumber: command.ReferenceNumber,
            purchaseOrderNumber: command.PurchaseOrderNumber,
            description: command.Description,
            notes: command.Notes,
            terms: command.Terms);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
