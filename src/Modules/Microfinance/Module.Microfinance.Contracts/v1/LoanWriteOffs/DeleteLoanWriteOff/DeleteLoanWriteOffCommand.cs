using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.DeleteLoanWriteOff;

public sealed record DeleteLoanWriteOffCommand(Guid Id) : ICommand;
