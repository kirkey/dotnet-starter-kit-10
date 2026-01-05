using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Bills.GetListBill;

/// <summary>
/// Get Bills (paginated) query.
/// </summary>
public record GetBillsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<BillsPagedResponse>;

/// <summary>
/// Paginated response for bills listing.
/// </summary>
public record BillsPagedResponse(
    List<BillSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for bill list items.
/// </summary>
