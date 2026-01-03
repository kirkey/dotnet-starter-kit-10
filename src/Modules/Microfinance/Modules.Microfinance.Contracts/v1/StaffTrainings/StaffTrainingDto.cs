namespace FSH.Modules.Microfinance.Contracts.v1.StaffTrainings;

public record StaffTrainingDto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record StaffTrainingSummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
