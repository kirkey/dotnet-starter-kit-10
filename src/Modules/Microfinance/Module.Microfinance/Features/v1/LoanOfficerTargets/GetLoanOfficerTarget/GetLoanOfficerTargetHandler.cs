using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets.GetLoanOfficerTarget;
using FSH.Module.Microfinance.Contracts.v1.LoanOfficerTargets;

namespace FSH.Module.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTarget;

public class GetLoanOfficerTargetHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanOfficerTargetQuery, LoanOfficerTargetDto>
{
    public async ValueTask<LoanOfficerTargetDto> Handle(GetLoanOfficerTargetQuery query, CancellationToken ct)
    {
        var entity = await context.LoanOfficerTargets
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanOfficerTargetDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanOfficerTarget not found");
    }
}
