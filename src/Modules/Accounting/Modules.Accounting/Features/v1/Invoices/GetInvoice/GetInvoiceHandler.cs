using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Invoices;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Invoices.GetInvoice;

public record GetInvoiceQuery(Guid Id) : IQuery<InvoiceDto>;

public class GetInvoiceHandler(AccountingDbContext context) : IQueryHandler<GetInvoiceQuery, InvoiceDto>
{
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
