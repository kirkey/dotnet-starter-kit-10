using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.SecurityDeposits;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.GetSecurityDeposit;

public record GetSecurityDepositQuery(Guid Id) : IQuery<SecurityDepositDto>;

public class GetSecurityDepositHandler(AccountingDbContext context) : IQueryHandler<GetSecurityDepositQuery, SecurityDepositDto>
{
    public async ValueTask<SecurityDepositDto> Handle(GetSecurityDepositQuery query, CancellationToken ct)
    {
        var entity = await context.SecurityDeposits
            .Where(x => x.Id == query.Id)
            .Select(x => new SecurityDepositDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("SecurityDeposit not found");
    }
}
