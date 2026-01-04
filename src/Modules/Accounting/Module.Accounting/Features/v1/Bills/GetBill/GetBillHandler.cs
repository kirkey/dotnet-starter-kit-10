using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Bills;
using FSH.Module.Accounting.Contracts.v1.Bills.GetBill;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Bills.GetBill;

public class GetBillHandler(AccountingDbContext context) : IQueryHandler<GetBillQuery, BillDto>
{
    public async ValueTask<BillDto> Handle(GetBillQuery query, CancellationToken ct)
    {
        var entity = await context.Bills
            .Where(x => x.Id == query.Id)
            .Select(x => new BillDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Bill not found");
    }
}
