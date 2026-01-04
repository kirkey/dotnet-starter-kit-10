using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Invoices.CreateInvoice;

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

/// <summary>
/// Handler for creating a new Invoice.
/// 
/// **Responsibility:**
/// Processes the CreateInvoiceCommand by creating a new Invoice aggregate with initial state.
/// Initializes line items collection and payment tracking (0 balance initially).
/// 
/// **Execution Flow:**
/// 1. Call Invoice.Create() factory method with command data
/// 2. Set customer or vendor reference based on invoice type
/// 3. Initialize line items collection (empty)
/// 4. Set SubTotal, Tax, Discount, Shipping to zero
/// 5. Set Status to "Draft"
/// 6. Record creation user and tenant
/// 7. Add to DbSet and persist
/// 8. Return new invoice ID
/// 
/// **Initial State:**
/// - Status: Draft (awaiting line items)
/// - IsPosted: false
/// - IsPaid: false
/// - AmountPaid: 0
/// - Amounts: Calculated from line items when added
/// - JournalEntryId: null (assigned when posted)
/// 
/// **Multi-Currency Support:**
/// - CurrencyCode: Stored with invoice for reporting
/// - ExchangeRate: Used for GL posting conversions
/// - Amounts: Stored in transaction currency
/// 
/// **Dependencies:**
/// - AccountingDbContext: For database persistence
/// - ICurrentUser: For accessing current user context
/// </summary>
public class CreateInvoiceHandler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<CreateInvoiceCommand, Guid>
{
    /// <summary>
    /// Handles the CreateInvoiceCommand to create a new invoice.
    /// </summary>
    /// <param name="command">The command containing invoice creation details</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The ID of the newly created invoice</returns>
    public async ValueTask<Guid> Handle(CreateInvoiceCommand command, CancellationToken ct)
    {
        var entity = Invoice.Create(
            command.InvoiceNumber,
            command.InvoiceDate,
            command.InvoiceType,
            command.DueDate,
            command.BillToName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.CustomerId,
            command.VendorId,
            command.BillToAddress,
            command.ShipToName,
            command.ShipToAddress,
            command.PaymentTerms,
            command.TaxCode,
            command.CurrencyCode,
            command.ExchangeRate,
            command.ReferenceNumber,
            command.PurchaseOrderNumber,
            command.Description,
            command.Notes,
            command.Terms);
        
        context.Invoices.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
