using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.InvoiceLineItems;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.InvoiceLineItems.GetInvoiceLineItem;

public record GetInvoiceLineItemQuery(Guid Id) : IQuery<InvoiceLineItemDto>;

public class GetInvoiceLineItemHandler(AccountingDbContext context) : IQueryHandler<GetInvoiceLineItemQuery, InvoiceLineItemDto>
{
    public async ValueTask<InvoiceLineItemDto> Handle(GetInvoiceLineItemQuery query, CancellationToken ct)
    {
        var entity = await context.InvoiceLineItems
            .Where(x => x.Id == query.Id)
            .Select(x => new InvoiceLineItemDto(
                x.Id,
                x.InvoiceId,
                x.LineNumber,
                x.ItemDescription,
                x.ItemCode,
                x.AccountId,
                x.AccountCode,
                x.Quantity,
                x.UnitOfMeasure,
                x.UnitPrice,
                x.DiscountPercent,
                x.DiscountAmount,
                x.LineTotal,
                x.TaxCode,
                x.TaxRate,
                x.TaxAmount,
                x.Notes,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InvoiceLineItem not found");
    }
}
