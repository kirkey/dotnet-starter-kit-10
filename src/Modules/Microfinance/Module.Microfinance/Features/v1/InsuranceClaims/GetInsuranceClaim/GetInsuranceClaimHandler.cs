using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.GetInsuranceClaim;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaim;

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
