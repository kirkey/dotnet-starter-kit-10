using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.GetQrPayment;
using FSH.Module.Microfinance.Contracts.v1.QrPayments;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.GetQrPayment;

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
