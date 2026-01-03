using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.LoanOfficerTargets;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTarget;

public record GetLoanOfficerTargetQuery(Guid Id) : IQuery<LoanOfficerTargetDto>;

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
