using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Invoices;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Invoices.GetInvoice;

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

/// <summary>
/// Handler for retrieving a single Invoice by ID.
/// 
/// **Responsibility:**
/// Queries the database for an Invoice by ID and returns it with all details.
/// Includes line item totals, payment tracking, and posting status.
/// 
/// **Execution Flow:**
/// 1. Query DbSet for Invoice by ID
/// 2. Project to InvoiceDto with all invoice properties
/// 3. Throw NotFoundException if not found
/// 4. Return complete DTO
/// 
/// **Returned Fields:**
/// - Id, InvoiceNumber, InvoiceDate, InvoiceType
/// - DueDate, CustomerId, VendorId
/// - BillToName, BillToAddress, ShipToName, ShipToAddress
/// - SubTotal, TaxAmount, DiscountAmount, ShippingAmount
/// - TotalAmount, AmountPaid, AmountDue
/// - PaymentTerms, TaxCode, CurrencyCode, ExchangeRate
/// - Status, IsPosted, IsPaid
/// - PostedDate, PaidDate, JournalEntryId
/// - ReferenceNumber, PurchaseOrderNumber
/// - Description, Notes, Terms
/// - IsActive, CreatedOnUtc
/// 
/// **Permissions:**
/// Requires: Accounting.Invoice.View
/// 
/// **Exceptions:**
/// - NotFoundException: Thrown when Invoice is not found
/// </summary>
public class GetInvoiceHandler(AccountingDbContext context) : IQueryHandler<GetInvoiceQuery, InvoiceDto>
{
    /// <summary>
    /// Handles the GetInvoiceQuery to retrieve an Invoice.
    /// </summary>
    /// <param name="query">The query containing the Invoice ID to retrieve</param>
    /// <param name="ct">Cancellation token for the operation</param>
    /// <returns>The InvoiceDto with complete invoice details</returns>
    /// <exception cref="NotFoundException">Thrown when Invoice with the specified ID is not found</exception>
    public async ValueTask<InvoiceDto> Handle(GetInvoiceQuery query, CancellationToken ct)
    {
        var entity = await context.Invoices
            .Where(x => x.Id == query.Id)
            .Select(x => new InvoiceDto(
                x.Id,
                x.InvoiceNumber,
                x.InvoiceDate,
                x.InvoiceType,
                x.DueDate,
                x.CustomerId,
                x.VendorId,
                x.BillToName,
                x.BillToAddress,
                x.ShipToName,
                x.ShipToAddress,
                x.SubTotal,
                x.TaxAmount,
                x.DiscountAmount,
                x.ShippingAmount,
                x.TotalAmount,
                x.AmountPaid,
                x.AmountDue,
                x.PaymentTerms,
                x.TaxCode,
                x.CurrencyCode,
                x.ExchangeRate,
                x.Status,
                x.IsPosted,
                x.IsPaid,
                x.PostedDate,
                x.PaidDate,
                x.JournalEntryId,
                x.ReferenceNumber,
                x.PurchaseOrderNumber,
                x.Description,
                x.Notes,
                x.Terms,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Invoice not found");
    }
}
