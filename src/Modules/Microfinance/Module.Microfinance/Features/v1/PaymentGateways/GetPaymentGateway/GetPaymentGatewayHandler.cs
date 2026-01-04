using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.PaymentGateways;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.PaymentGateways.GetPaymentGateway;

public record GetPaymentGatewayQuery(Guid Id) : IQuery<PaymentGatewayDto>;

public class GetPaymentGatewayHandler(MicrofinanceDbContext context) : IQueryHandler<GetPaymentGatewayQuery, PaymentGatewayDto>
{
    public async ValueTask<PaymentGatewayDto> Handle(GetPaymentGatewayQuery query, CancellationToken ct)
    {
        var entity = await context.PaymentGateways
            .Where(x => x.Id == query.Id)
            .Select(x => new PaymentGatewayDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PaymentGateway not found");
    }
}
