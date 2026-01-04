using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccount;

public record GetChartOfAccountQuery(Guid Id) : IQuery<ChartOfAccountDto>;

public class GetChartOfAccountHandler(AccountingDbContext context) : IQueryHandler<GetChartOfAccountQuery, ChartOfAccountDto>
{
    public async ValueTask<ChartOfAccountDto> Handle(GetChartOfAccountQuery query, CancellationToken ct)
    {
        var entity = await context.ChartOfAccounts
            .Where(x => x.Id == query.Id)
            .Select(x => new ChartOfAccountDto(
                x.Id,
                x.AccountCode,
                x.AccountName,
                x.AccountType,
                x.UsoaCategory,
                x.ParentAccountId,
                x.ParentCode,
                x.Balance,
                x.IsControlAccount,
                x.NormalBalance,
                x.AccountLevel,
                x.AllowDirectPosting,
                x.IsUsoaCompliant,
                x.RegulatoryClassification,
                x.Description,
                x.Notes,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ChartOfAccount not found");
    }
}
