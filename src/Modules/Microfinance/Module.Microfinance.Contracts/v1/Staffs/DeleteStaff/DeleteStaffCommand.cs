using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Staffs.DeleteStaff;

public sealed record DeleteStaffCommand(Guid Id) : ICommand;
