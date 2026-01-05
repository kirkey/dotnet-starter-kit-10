using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanSchedules.UpdateLoanSchedule;

public sealed record UpdateLoanScheduleCommand(Guid Id, string Name) : ICommand<Guid>;
