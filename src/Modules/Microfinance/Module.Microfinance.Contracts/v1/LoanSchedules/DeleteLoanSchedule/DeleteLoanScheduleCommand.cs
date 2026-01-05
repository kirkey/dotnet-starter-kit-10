using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanSchedules.DeleteLoanSchedule;

public sealed record DeleteLoanScheduleCommand(Guid Id) : ICommand;
