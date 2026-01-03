using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.InsurancePolicys;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.InsurancePolicys.GetInsurancePolicy;

public record GetInsurancePolicyQuery(Guid Id) : IQuery<InsurancePolicyDto>;

public class GetInsurancePolicyHandler(MicrofinanceDbContext context) : IQueryHandler<GetInsurancePolicyQuery, InsurancePolicyDto>
{
    public async ValueTask<InsurancePolicyDto> Handle(GetInsurancePolicyQuery query, CancellationToken ct)
    {
        var entity = await context.InsurancePolicys
            .Where(x => x.Id == query.Id)
            .Select(x => new InsurancePolicyDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InsurancePolicy not found");
    }
}
