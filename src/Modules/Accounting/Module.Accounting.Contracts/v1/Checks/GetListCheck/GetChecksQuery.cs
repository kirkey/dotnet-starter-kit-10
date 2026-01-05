using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Checks.GetListCheck;

/// <summary>
/// Get Checks (paginated) query to retrieve a paginated list of checks with optional filtering by multiple criteria.
/// </summary>
/// <param name="Page">Page number for pagination (1-based, default=1)</param>
/// <param name="PageSize">Number of items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by CheckNumber, PayeeName, or AccountNumber (contains search)</param>
/// <param name="IsActive">Optional filter by active status (null = all)</param>
/// <param name="Status">Optional filter by check status (e.g., "Draft", "Printed", "Issued", "Cleared", "Voided")</param>
/// <param name="FromDate">Optional filter by check date (inclusive, greater than or equal)</param>
/// <param name="ToDate">Optional filter by check date (inclusive, less than or equal)</param>
public record GetChecksQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null,
    string? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null) : IQuery<ChecksPagedResponse>;

/// <summary>
/// Response object for paginated check list with summary data.
/// </summary>
/// <param name="Items">List of CheckSummaryDto with summary fields</param>
/// <param name="TotalCount">Total count of checks matching filters (excluding pagination)</param>
/// <param name="Page">Requested page number</param>
/// <param name="PageSize">Items per page</param>
public record ChecksPagedResponse(
    List<CheckSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for check list responses with essential check information.
/// </summary>
/// <param name="Id">Check ID</param>
/// <param name="CheckNumber">Check number</param>
/// <param name="CheckDate">Check date</param>
/// <param name="PayeeName">Payee name</param>
/// <param name="Amount">Check amount</param>
/// <param name="Status">Check status</param>
/// <param name="IsActive">Active flag</param>
public record CheckSummaryDto(
    Guid Id,
    string CheckNumber,
    DateTime CheckDate,
    string PayeeName,
    decimal Amount,
    string Status,
    bool IsActive);