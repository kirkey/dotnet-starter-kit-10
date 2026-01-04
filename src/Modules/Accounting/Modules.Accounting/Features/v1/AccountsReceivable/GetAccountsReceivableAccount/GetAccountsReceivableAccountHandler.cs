using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.AccountsReceivable;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivableAccount;

public record GetAccountsReceivableAccountQuery(Guid Id) : IQuery<AccountsReceivableAccountDto>;

public class GetAccountsReceivableAccountHandler(AccountingDbContext context) : IQueryHandler<GetAccountsReceivableAccountQuery, AccountsReceivableAccountDto>
{
    public async ValueTask<AccountsReceivableAccountDto> Handle(GetAccountsReceivableAccountQuery query, CancellationToken ct)
    {
        var entity = await context.AccountsReceivable
            .Where(x => x.Id == query.Id)
            .Select(x => new AccountsReceivableAccountDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("AccountsReceivableAccount not found");
    }
}
