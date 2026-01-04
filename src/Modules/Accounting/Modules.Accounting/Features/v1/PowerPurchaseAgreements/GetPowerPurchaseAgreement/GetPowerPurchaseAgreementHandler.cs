using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.PowerPurchaseAgreements;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreement;

public record GetPowerPurchaseAgreementQuery(Guid Id) : IQuery<PowerPurchaseAgreementDto>;

public class GetPowerPurchaseAgreementHandler(AccountingDbContext context) : IQueryHandler<GetPowerPurchaseAgreementQuery, PowerPurchaseAgreementDto>
{
    public async ValueTask<PowerPurchaseAgreementDto> Handle(GetPowerPurchaseAgreementQuery query, CancellationToken ct)
    {
        var entity = await context.PowerPurchaseAgreements
            .Where(x => x.Id == query.Id)
            .Select(x => new PowerPurchaseAgreementDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("PowerPurchaseAgreement not found");
    }
}
