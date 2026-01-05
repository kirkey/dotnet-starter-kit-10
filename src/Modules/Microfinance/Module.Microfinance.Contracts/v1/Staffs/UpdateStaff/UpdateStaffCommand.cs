using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Staffs.UpdateStaff;

public sealed record UpdateStaffCommand(Guid Id, string Name) : ICommand<Guid>;
