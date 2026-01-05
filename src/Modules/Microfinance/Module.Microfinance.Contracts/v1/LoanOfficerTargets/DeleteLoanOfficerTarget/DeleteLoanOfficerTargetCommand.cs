using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.DeleteLoanOfficerTarget;

public sealed record DeleteLoanOfficerTargetCommand(Guid Id) : ICommand;
