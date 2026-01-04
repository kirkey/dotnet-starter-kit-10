namespace Accounting.Application.Meters.Create.v1;

/// <summary>
/// Command to create a new meter.
/// </summary>
public sealed record CreateMeterCommand(
    string MeterNumber,
    string MeterType,
    string Manufacturer,
    string ModelNumber,
    DateTime InstallationDate,
    decimal Multiplier,
    string? SerialNumber,
    string? Location,
    string? GpsCoordinates,
    DefaultIdType? MemberId,
    bool IsSmartMeter,
    string? CommunicationProtocol,
    decimal? AccuracyClass,
    string? MeterConfiguration,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;

