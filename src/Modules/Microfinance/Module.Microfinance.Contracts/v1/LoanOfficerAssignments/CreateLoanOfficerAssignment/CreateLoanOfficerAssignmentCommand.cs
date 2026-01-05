using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.CreateLoanOfficerAssignment;

public sealed record CreateLoanOfficerAssignmentCommand(string Name) : ICommand<Guid>;
