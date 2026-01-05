using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.GetCustomerCase;
using FSH.Module.Microfinance.Contracts.v1.CustomerCases;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.GetCustomerCase;

public class GetCustomerCaseHandler(MicrofinanceDbContext context) : IQueryHandler<GetCustomerCaseQuery, CustomerCaseDto>
{
    public async ValueTask<CustomerCaseDto> Handle(GetCustomerCaseQuery query, CancellationToken ct)
    {
        var entity = await context.CustomerCases
            .Where(x => x.Id == query.Id)
            .Select(x => new CustomerCaseDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CustomerCase not found");
    }
}
