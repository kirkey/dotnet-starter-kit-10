using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.PaymentAllocations;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PaymentAllocations.GetPaymentAllocation;

public record GetPaymentAllocationQuery(Guid Id) : IQuery<PaymentAllocationDto>;

public class GetPaymentAllocationHandler(AccountingDbContext context) : IQueryHandler<GetPaymentAllocationQuery, PaymentAllocationDto>
{
    public async ValueTask<PaymentAllocationDto> Handle(GetPaymentAllocationQuery query, CancellationToken ct)
    {
        var entity = await context.PaymentAllocations
            .Where(x => x.Id == query.Id)
            .Select(x => new PaymentAllocationDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PaymentAllocation not found");
    }
}
