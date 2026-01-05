using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.UpdateLoanWriteOff;

public sealed record UpdateLoanWriteOffCommand(Guid Id, string Name) : ICommand<Guid>;
