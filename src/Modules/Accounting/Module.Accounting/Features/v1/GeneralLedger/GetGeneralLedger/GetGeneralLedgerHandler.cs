using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.GeneralLedger;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.GeneralLedger.GetGeneralLedger;

public record GetGeneralLedgerQuery(Guid Id) : IQuery<GeneralLedgerDto>;

public class GetGeneralLedgerHandler(AccountingDbContext context) : IQueryHandler<GetGeneralLedgerQuery, GeneralLedgerDto>
{
    public async ValueTask<GeneralLedgerDto> Handle(GetGeneralLedgerQuery query, CancellationToken ct)
    {
        var entity = await context.GeneralLedger
            .Where(x => x.Id == query.Id)
            .Select(x => new GeneralLedgerDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("GeneralLedger not found");
    }
}
