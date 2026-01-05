using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.GetLoanOfficerAssignment;

public sealed record GetLoanOfficerAssignmentQuery(Guid Id) : IQuery<LoanOfficerAssignmentDto>;
