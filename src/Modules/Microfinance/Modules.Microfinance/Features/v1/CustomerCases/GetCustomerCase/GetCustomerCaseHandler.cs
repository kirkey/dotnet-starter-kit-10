using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.CustomerCases;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CustomerCases.GetCustomerCase;

public record GetCustomerCaseQuery(Guid Id) : IQuery<CustomerCaseDto>;

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
