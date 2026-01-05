using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Staffs.GetStaff;

public sealed record GetStaffQuery(Guid Id) : IQuery<StaffDto>;
