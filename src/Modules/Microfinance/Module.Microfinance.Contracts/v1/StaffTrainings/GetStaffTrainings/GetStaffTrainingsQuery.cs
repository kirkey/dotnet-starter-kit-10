using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.StaffTrainings.GetStaffTrainings;

public sealed record GetStaffTrainingsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<StaffTrainingsPagedResponse>;
