using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanSchedules.GetLoanSchedule;

public sealed record GetLoanScheduleQuery(Guid Id) : IQuery<LoanScheduleDto>;
