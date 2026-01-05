using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;
using FSH.Module.Accounting.Contracts.v1.AccountsPayable.GetAccountsPayableAccount;

namespace FSH.Module.Accounting.Features.v1.AccountsPayable.GetAccountsPayableAccount;

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
