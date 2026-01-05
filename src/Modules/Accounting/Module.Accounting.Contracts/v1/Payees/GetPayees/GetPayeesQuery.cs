using FSH.Module.Accounting.Contracts.v1.Payees;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payees.GetPayees;

/// <summary>
/// Query to retrieve a paginated list of payees.
/// </summary>
/// <param name="Page">Page number (1-based, default=1)</param>
/// <param name="PageSize">Items per page (default=10)</param>
/// <param name="SearchTerm">Optional filter by payee Name</param>
/// <param name="IsActive">Optional filter by active status</param>
public record GetPayeesQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PayeesPagedResponse>;

/// <summary>
/// Response for paginated payee list.
/// </summary>
public record PayeesPagedResponse(
    List<PayeeSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

