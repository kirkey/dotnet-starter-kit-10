using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.CreateLoanOfficerTarget;

public sealed record CreateLoanOfficerTargetCommand(string Name) : ICommand<Guid>;
