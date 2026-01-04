namespace Accounting.Application.Consumptions.Update.v1;

/// <summary>
/// Command to update a consumption record.
/// </summary>
public sealed record UpdateConsumptionCommand(
    DefaultIdType Id,
    string? ReadingType,
    string? ReadingSource,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;

