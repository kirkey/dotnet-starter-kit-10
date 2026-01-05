using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.RegulatoryReports;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.RegulatoryReports.GetRegulatoryReport;

namespace FSH.Module.Accounting.Features.v1.RegulatoryReports.GetRegulatoryReport;

public class GetRegulatoryReportHandler(AccountingDbContext context) : IQueryHandler<GetRegulatoryReportQuery, RegulatoryReportDto>
{
    public async ValueTask<RegulatoryReportDto> Handle(GetRegulatoryReportQuery query, CancellationToken ct)
    {
        var entity = await context.RegulatoryReports
            .Where(x => x.Id == query.Id)
            .Select(x => new RegulatoryReportDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("RegulatoryReport not found");
    }
}
