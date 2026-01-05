using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Staffs.CreateStaff;

public sealed record CreateStaffCommand(string Name) : ICommand<Guid>;
