using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CreditScores;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CreditScores.GetCreditScore;

public record GetCreditScoreQuery(Guid Id) : IQuery<CreditScoreDto>;

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
