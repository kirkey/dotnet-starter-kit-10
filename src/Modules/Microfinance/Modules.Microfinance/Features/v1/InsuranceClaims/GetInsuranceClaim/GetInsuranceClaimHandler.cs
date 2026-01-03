using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.InsuranceClaims;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaim;

public record GetInsuranceClaimQuery(Guid Id) : IQuery<InsuranceClaimDto>;

public class GetInsuranceClaimHandler(MicrofinanceDbContext context) : IQueryHandler<GetInsuranceClaimQuery, InsuranceClaimDto>
{
    public async ValueTask<InsuranceClaimDto> Handle(GetInsuranceClaimQuery query, CancellationToken ct)
    {
        var entity = await context.InsuranceClaims
            .Where(x => x.Id == query.Id)
            .Select(x => new InsuranceClaimDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InsuranceClaim not found");
    }
}
