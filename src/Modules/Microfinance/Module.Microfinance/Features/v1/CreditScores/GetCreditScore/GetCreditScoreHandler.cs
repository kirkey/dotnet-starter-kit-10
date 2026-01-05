using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CreditScores.GetCreditScore;
using FSH.Module.Microfinance.Contracts.v1.CreditScores;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.GetCreditScore;

public class GetCreditScoreHandler(MicrofinanceDbContext context) : IQueryHandler<GetCreditScoreQuery, CreditScoreDto>
{
    public async ValueTask<CreditScoreDto> Handle(GetCreditScoreQuery query, CancellationToken ct)
    {
        var entity = await context.CreditScores
            .Where(x => x.Id == query.Id)
            .Select(x => new CreditScoreDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CreditScore not found");
    }
}
