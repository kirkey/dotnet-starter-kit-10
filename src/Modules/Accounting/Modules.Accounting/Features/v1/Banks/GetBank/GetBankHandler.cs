using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Banks;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Banks.GetBank;

public record GetBankQuery(Guid Id) : IQuery<BankDto>;

public class GetBankHandler(AccountingDbContext context) : IQueryHandler<GetBankQuery, BankDto>
{
    public async ValueTask<BankDto> Handle(GetBankQuery query, CancellationToken ct)
    {
        var entity = await context.Banks
            .Where(x => x.Id == query.Id)
            .Select(x => new BankDto(
                x.Id,
                x.BankName,
                x.BankCode,
                x.Address,
                x.ContactName,
                x.ContactPhone,
                x.RoutingNumber,
                x.SwiftCode,
                x.CurrencyCode,
                x.OpeningBalance,
                x.CurrentBalance,
                x.IsDefault,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Bank not found");
    }
}
