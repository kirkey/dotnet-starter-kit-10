using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.InterconnectionAgreements;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.InterconnectionAgreements.GetInterconnectionAgreement;

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
