using FSH.Module.Accounting.Contracts.v1.Invoices;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Invoices.GetInvoices;

/// <summary>
/// Query to retrieve a paginated list of invoices with optional filtering by multiple criteria.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by InvoiceNumber, BillToName, or ReferenceNumber (contains search)</param>
/// <param name="IsActive">Optional filter by active status (null = all)</param>
/// <param name="InvoiceType">Optional filter by invoice type (e.g., "Sales", "Purchase")</param>
/// <param name="Status">Optional filter by invoice status (e.g., "Draft", "Posted", "Approved", "Paid")</param>
/// <param name="IsPaid">Optional filter by payment status (true = fully paid, false = unpaid/partial, null = all)</param>
/// <param name="FromDate">Optional filter by invoice date >= FromDate (inclusive)</param>
/// <param name="ToDate">Optional filter by invoice date <= ToDate (inclusive)</param>
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

/// <summary>
/// Response object for paginated invoice list with summary data.
/// </summary>
/// <param name="Items">List of InvoiceSummaryDto with summary fields</param>
/// <param name="TotalCount">Total count of invoices matching filters (excluding pagination)</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record InvoicesPagedResponse(
    List<InvoiceSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Handler for retrieving a paginated, filtered list of invoices with complex filtering and workflow support.
/// </summary>
/// <remarks>
/// Responsibility: Query invoices with 9-parameter filtering, pagination, and summary projection.
/// 
/// Execution Flow:
/// 1. Build queryable from Invoices DbSet
/// 2. Apply SearchTerm filter (multi-field): InvoiceNumber OR BillToName OR ReferenceNumber (contains) if provided
/// 3. Apply IsActive filter using Where(x => x.IsActive == value) if provided
/// 4. Apply InvoiceType filter using Where(x => x.InvoiceType == value) if provided (workflow classification)
/// 5. Apply Status filter using Where(x => x.Status == value) if provided (workflow status)
/// 6. Apply IsPaid filter using Where(x => x.IsPaid == value) if provided (payment status)
/// 7. Apply FromDate filter using Where(x => x.InvoiceDate >= FromDate) if provided (inclusive)
/// 8. Apply ToDate filter using Where(x => x.InvoiceDate <= ToDate) if provided (inclusive)
/// 9. Get total count before pagination using CountAsync
/// 10. Sort by InvoiceDate in descending order (most recent first) for aging analysis
/// 11. Apply pagination using Skip((page-1)*pageSize).Take(pageSize)
/// 12. Project to InvoiceSummaryDto with summary fields
/// 13. Execute query and return InvoicesPagedResponse
/// 
/// Filtering Logic:
/// - SearchTerm: Case-insensitive multi-field search (InvoiceNumber, BillToName, ReferenceNumber)
/// - IsActive: Exact match on IsActive boolean (null = no filter)
/// - InvoiceType: Exact match (e.g., "Sales Invoice", "Purchase Invoice" - workflow classification)
/// - Status: Exact match (Draft, Posted, Approved, Paid - invoice lifecycle status)
/// - IsPaid: Exact match on IsPaid boolean (null = no filter, true = fully paid, false = unpaid/partial)
/// - FromDate: Inclusive >= comparison on InvoiceDate (period start, typically accounting month)
/// - ToDate: Inclusive <= comparison on InvoiceDate (period end, typically accounting month)
/// 
/// Sorting: InvoiceDate DESC (most recent invoices first for aging reports)
/// 
/// Pagination: Standard skip-take pattern (page-1)*pageSize
/// 
/// Returned Fields (InvoiceSummaryDto): Id, InvoiceNumber, InvoiceDate, BillToName, Total, Status, IsPaid
/// 
/// Use Cases:
/// - Accounts Receivable aging report (InvoiceType='Sales', Status='Posted', FromDate/ToDate filter)
/// - Outstanding invoices list (Status != 'Paid', IsPaid=false, sorted by InvoiceDate ASC for follow-up)
/// - Accounts Payable aging (InvoiceType='Purchase', Status='Posted', IsPaid=false)
/// - Invoice search (SearchTerm filter by number/customer/reference)
/// - Period-end reporting (FromDate/ToDate range with Status filter)
/// 
/// Permissions: Requires authenticated user
/// 
/// Exceptions: None; returns empty list if no matches found
/// </remarks>
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
