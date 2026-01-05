using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.UpdateLoanOfficerTarget;

public sealed record UpdateLoanOfficerTargetCommand(Guid Id, string Name) : ICommand<Guid>;
