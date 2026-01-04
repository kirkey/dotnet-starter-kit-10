using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.TaxCodes;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.GetTaxCode;

public record GetTaxCodeQuery(Guid Id) : IQuery<TaxCodeDto>;

public class GetTaxCodeHandler(AccountingDbContext context) : IQueryHandler<GetTaxCodeQuery, TaxCodeDto>
{
    public async ValueTask<TaxCodeDto> Handle(GetTaxCodeQuery query, CancellationToken ct)
    {
        var entity = await context.TaxCodes
            .Where(x => x.Id == query.Id)
            .Select(x => new TaxCodeDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("TaxCode not found");
    }
}
