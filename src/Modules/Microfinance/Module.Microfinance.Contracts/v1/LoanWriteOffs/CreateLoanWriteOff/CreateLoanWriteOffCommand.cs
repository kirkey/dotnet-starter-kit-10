using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.CreateLoanWriteOff;

public sealed record CreateLoanWriteOffCommand(string Name) : ICommand<Guid>;
