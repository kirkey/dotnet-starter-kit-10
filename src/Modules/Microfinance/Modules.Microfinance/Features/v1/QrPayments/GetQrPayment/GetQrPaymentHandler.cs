using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.QrPayments;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.QrPayments.GetQrPayment;

public record GetQrPaymentQuery(Guid Id) : IQuery<QrPaymentDto>;

public class GetQrPaymentHandler(MicrofinanceDbContext context) : IQueryHandler<GetQrPaymentQuery, QrPaymentDto>
{
    public async ValueTask<QrPaymentDto> Handle(GetQrPaymentQuery query, CancellationToken ct)
    {
        var entity = await context.QrPayments
            .Where(x => x.Id == query.Id)
            .Select(x => new QrPaymentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("QrPayment not found");
    }
}
