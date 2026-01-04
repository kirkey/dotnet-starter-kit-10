using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.GetCreditBureauReport;

public record GetCreditBureauReportQuery(Guid Id) : IQuery<CreditBureauReportDto>;

public class GetCreditBureauReportHandler(MicrofinanceDbContext context) : IQueryHandler<GetCreditBureauReportQuery, CreditBureauReportDto>
{
    public async ValueTask<CreditBureauReportDto> Handle(GetCreditBureauReportQuery query, CancellationToken ct)
    {
        var entity = await context.CreditBureauReports
            .Where(x => x.Id == query.Id)
            .Select(x => new CreditBureauReportDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("CreditBureauReport not found");
    }
}
