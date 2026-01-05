using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Payees;
using FSH.Module.Accounting.Contracts.v1.Payees.GetPayee;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Payees.GetPayee;

public class GetPayeeHandler(AccountingDbContext context) : IQueryHandler<GetPayeeQuery, PayeeDto>
{
    public async ValueTask<PayeeDto> Handle(GetPayeeQuery query, CancellationToken ct)
    {
        var entity = await context.Payees
            .Where(x => x.Id == query.Id)
            .Select(x => new PayeeDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Payee not found");
    }
}
