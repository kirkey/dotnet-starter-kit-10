using FSH.Modules.Accounting.Contracts.v1.Invoices;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Invoices.GetInvoices;

public record GetInvoicesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? InvoiceType = null,
    string? Status = null,
    bool? IsPaid = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<InvoicesPagedResponse>;

public record InvoicesPagedResponse(
    List<InvoiceSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class GetInvoicesHandler(AccountingDbContext context) 
    : IQueryHandler<GetInvoicesQuery, InvoicesPagedResponse>
{
    public async ValueTask<InvoicesPagedResponse> Handle(GetInvoicesQuery query, CancellationToken ct)
    {
        var queryable = context.Invoices.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => 
                x.InvoiceNumber.Contains(query.SearchTerm) ||
                x.BillToName.Contains(query.SearchTerm) ||
                (x.ReferenceNumber != null && x.ReferenceNumber.Contains(query.SearchTerm)));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(query.InvoiceType))
        {
            queryable = queryable.Where(x => x.InvoiceType == query.InvoiceType);
        }
        
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            queryable = queryable.Where(x => x.Status == query.Status);
        }
        
        if (query.IsPaid.HasValue)
        {
            queryable = queryable.Where(x => x.IsPaid == query.IsPaid.Value);
        }
        
        if (query.FromDate.HasValue)
        {
            queryable = queryable.Where(x => x.InvoiceDate >= query.FromDate.Value);
        }
        
        if (query.ToDate.HasValue)
        {
            queryable = queryable.Where(x => x.InvoiceDate <= query.ToDate.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.InvoiceDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new InvoiceSummaryDto(
                x.Id,
                x.InvoiceNumber,
                x.InvoiceDate,
                x.InvoiceType,
                x.DueDate,
                x.BillToName,
                x.TotalAmount,
                x.AmountDue,
                x.Status,
                x.IsPaid,
                x.IsActive))
            .ToListAsync(ct);
        
        return new InvoicesPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
