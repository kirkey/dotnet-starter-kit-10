namespace Accounting.Application.Meters.Update.v1;

/// <summary>
/// Command to update a meter.
/// </summary>
public sealed record UpdateMeterCommand(
    DefaultIdType Id,
    string? Location,
    string? GpsCoordinates,
    DefaultIdType? MemberId,
    string? CommunicationProtocol,
    string? MeterConfiguration,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;

