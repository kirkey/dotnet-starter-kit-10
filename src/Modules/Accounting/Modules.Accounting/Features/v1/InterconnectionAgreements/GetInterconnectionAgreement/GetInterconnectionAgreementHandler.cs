using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.InterconnectionAgreements;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.InterconnectionAgreements.GetInterconnectionAgreement;

public record GetInterconnectionAgreementQuery(Guid Id) : IQuery<InterconnectionAgreementDto>;

public class GetInterconnectionAgreementHandler(AccountingDbContext context) : IQueryHandler<GetInterconnectionAgreementQuery, InterconnectionAgreementDto>
{
    public async ValueTask<InterconnectionAgreementDto> Handle(GetInterconnectionAgreementQuery query, CancellationToken ct)
    {
        var entity = await context.InterconnectionAgreements
            .Where(x => x.Id == query.Id)
            .Select(x => new InterconnectionAgreementDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("InterconnectionAgreement not found");
    }
}
