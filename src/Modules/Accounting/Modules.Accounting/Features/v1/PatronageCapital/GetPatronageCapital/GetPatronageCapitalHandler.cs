using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.PatronageCapital;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PatronageCapital.GetPatronageCapital;

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
