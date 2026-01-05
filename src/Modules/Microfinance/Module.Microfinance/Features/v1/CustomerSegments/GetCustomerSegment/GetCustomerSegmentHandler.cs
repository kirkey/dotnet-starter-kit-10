using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CustomerSegments;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CustomerSegments.GetCustomerSegment;

namespace FSH.Module.Microfinance.Features.v1.CustomerSegments.GetCustomerSegment;

public class GetCustomerSegmentHandler(MicrofinanceDbContext context) : IQueryHandler<GetCustomerSegmentQuery, CustomerSegmentDto>
{
    public async ValueTask<CustomerSegmentDto> Handle(GetCustomerSegmentQuery query, CancellationToken ct)
    {
        var entity = await context.CustomerSegments
            .Where(x => x.Id == query.Id)
            .Select(x => new CustomerSegmentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CustomerSegment not found");
    }
}
