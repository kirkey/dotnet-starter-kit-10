namespace Accounting.Application.DeferredRevenues.Update;

/// <summary>
/// Command to update an existing deferred revenue entry.
/// </summary>
public sealed record UpdateDeferredRevenueCommand(
    DefaultIdType Id,
    DateTime? RecognitionDate,
    decimal? Amount,
    string? Description) : IRequest<DefaultIdType>;

