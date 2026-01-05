using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.AccountsReceivable;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.AccountsReceivable.GetAccountsReceivableAccount;namespace FSH.Module.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivableAccount;

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
