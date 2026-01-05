using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.UpdateLoanOfficerAssignment;

public sealed record UpdateLoanOfficerAssignmentCommand(Guid Id, string Name) : ICommand<Guid>;
