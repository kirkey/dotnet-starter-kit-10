using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanSchedules.CreateLoanSchedule;

public sealed record CreateLoanScheduleCommand(string Name) : ICommand<Guid>;
