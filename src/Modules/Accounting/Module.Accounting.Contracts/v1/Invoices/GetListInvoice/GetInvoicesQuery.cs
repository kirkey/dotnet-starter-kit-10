using FSH.Module.Accounting.Contracts.v1.Invoices;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Invoices.GetListInvoice;

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
/// <param name="FromDate">Optional filter by invoice date greater than or equal to FromDate (inclusive)</param>
/// <param name="ToDate">Optional filter by invoice date less than or equal to ToDate (inclusive)</param>
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
/// Paginated response for invoices listing.
/// </summary>
public record InvoicesPagedResponse(
    List<InvoiceSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for invoice list items.
/// </summary>
