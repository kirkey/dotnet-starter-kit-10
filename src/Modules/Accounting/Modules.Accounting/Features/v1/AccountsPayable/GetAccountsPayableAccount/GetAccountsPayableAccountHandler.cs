using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.AccountsPayable;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.AccountsPayable.GetAccountsPayableAccount;

public record GetAccountsPayableAccountQuery(Guid Id) : IQuery<AccountsPayableAccountDto>;

public class GetAccountsPayableAccountHandler(AccountingDbContext context) : IQueryHandler<GetAccountsPayableAccountQuery, AccountsPayableAccountDto>
{
    public async ValueTask<AccountsPayableAccountDto> Handle(GetAccountsPayableAccountQuery query, CancellationToken ct)
    {
        var entity = await context.AccountsPayable
            .Where(x => x.Id == query.Id)
            .Select(x => new AccountsPayableAccountDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("AccountsPayableAccount not found");
    }
}
