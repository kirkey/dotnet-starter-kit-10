using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payments;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Payments.GetPayment;

public record GetPaymentQuery(Guid Id) : IQuery<PaymentDto>;

public class GetPaymentHandler(AccountingDbContext context) : IQueryHandler<GetPaymentQuery, PaymentDto>
{
    public async ValueTask<PaymentDto> Handle(GetPaymentQuery query, CancellationToken ct)
    {
        var entity = await context.Payments
            .Where(x => x.Id == query.Id)
            .Select(x => new PaymentDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Payment not found");
    }
}
