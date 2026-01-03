namespace FSH.Modules.Microfinance.Contracts.v1.StaffTrainings;

public record GetStaffTrainingQuery(Guid Id);
public record GetStaffTrainingsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record StaffTrainingsPagedResponse(List<StaffTrainingSummaryDto> Items, int TotalCount, int Page, int PageSize);
