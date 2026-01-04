using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.PatronageCapital;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.GetPatronageCapital;

public record GetPatronageCapitalQuery(Guid Id) : IQuery<PatronageCapitalDto>;

public class GetPatronageCapitalHandler(AccountingDbContext context) : IQueryHandler<GetPatronageCapitalQuery, PatronageCapitalDto>
{
    public async ValueTask<PatronageCapitalDto> Handle(GetPatronageCapitalQuery query, CancellationToken ct)
    {
        var entity = await context.PatronageCapital
            .Where(x => x.Id == query.Id)
            .Select(x => new PatronageCapitalDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PatronageCapital not found");
    }
}
