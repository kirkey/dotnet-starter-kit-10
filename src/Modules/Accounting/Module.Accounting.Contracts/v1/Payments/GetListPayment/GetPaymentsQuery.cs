using FSH.Module.Accounting.Contracts.v1.Payments;
using Mediator;

namespace FSH.Module.Accounting.Contracts.v1.Payments.GetListPayment;

/// <summary>
/// Get Payments (paginated) query.
/// </summary>
public record GetPaymentsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<PaymentsPagedResponse>;

/// <summary>
/// Paginated response for payments listing.
/// </summary>
public record PaymentsPagedResponse(
    List<PaymentSummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

/// <summary>
/// Summary DTO for payment list items.
/// </summary>
public record PaymentSummaryDto(Guid Id, string Name, bool IsActive);
