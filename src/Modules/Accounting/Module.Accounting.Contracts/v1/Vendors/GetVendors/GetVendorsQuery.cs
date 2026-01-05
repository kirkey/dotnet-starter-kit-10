using FSH.Module.Accounting.Contracts.v1.Vendors;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Vendors.GetVendors;

/// <summary>
/// Query to retrieve a paginated list of vendors with optional filtering.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by vendor Name (contains search)</param>
/// <param name="IsActive">Optional filter by active status (null = all, true = active only, false = inactive only)</param>
public record GetVendorsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<VendorsPagedResponse>;

/// <summary>
/// Response object for paginated vendor list with summary data.
/// </summary>
/// <param name="Items">List of VendorSummaryDto with 3 returned fields: Id, Name, IsActive</param>
/// <param name="TotalCount">Total count of vendors matching filters (excluding pagination)</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record VendorsPagedResponse(
    List<VendorSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary projection of a vendor for list views.
/// </summary>
/// <param name="Id">Vendor unique identifier</param>
/// <param name="Name">Vendor business name</param>
/// <param name="IsActive">Whether vendor is currently active</param>
public record VendorSummaryDto(Guid Id, string Name, bool IsActive);