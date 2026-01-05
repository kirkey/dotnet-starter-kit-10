using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.DeleteLoanOfficerAssignment;

public sealed record DeleteLoanOfficerAssignmentCommand(Guid Id) : ICommand;
