using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments.GetLoanOfficerAssignment;
using FSH.Module.Microfinance.Contracts.v1.LoanOfficerAssignments;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerAssignments.GetLoanOfficerAssignment;

public class GetLoanOfficerAssignmentHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanOfficerAssignmentQuery, LoanOfficerAssignmentDto>
{
    public async ValueTask<LoanOfficerAssignmentDto> Handle(GetLoanOfficerAssignmentQuery query, CancellationToken ct)
    {
        var entity = await context.LoanOfficerAssignments
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanOfficerAssignmentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanOfficerAssignment not found");
    }
}
