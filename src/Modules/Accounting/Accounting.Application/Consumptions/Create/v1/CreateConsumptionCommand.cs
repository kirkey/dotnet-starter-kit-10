namespace Accounting.Application.Consumptions.Create.v1;

/// <summary>
/// Command to create a new consumption record.
/// </summary>
public sealed record CreateConsumptionCommand(
    DefaultIdType MeterId,
    DateTime ReadingDate,
    decimal CurrentReading,
    decimal PreviousReading,
    string BillingPeriod,
    string ReadingType,
    decimal? Multiplier,
    string? ReadingSource,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;

