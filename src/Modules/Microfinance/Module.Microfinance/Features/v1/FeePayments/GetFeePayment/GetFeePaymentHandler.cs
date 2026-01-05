using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.GetFeePayment;
using FSH.Module.Microfinance.Contracts.v1.FeePayments;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.GetFeePayment;

public class GetFeePaymentHandler(MicrofinanceDbContext context) : IQueryHandler<GetFeePaymentQuery, FeePaymentDto>
{
    public async ValueTask<FeePaymentDto> Handle(GetFeePaymentQuery query, CancellationToken ct)
    {
        var entity = await context.FeePayments
            .Where(x => x.Id == query.Id)
            .Select(x => new FeePaymentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("FeePayment not found");
    }
}
