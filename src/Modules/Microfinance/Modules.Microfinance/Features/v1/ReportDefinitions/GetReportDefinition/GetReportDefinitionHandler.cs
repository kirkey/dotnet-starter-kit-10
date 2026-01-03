using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.ReportDefinitions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ReportDefinitions.GetReportDefinition;

public record GetReportDefinitionQuery(Guid Id) : IQuery<ReportDefinitionDto>;

public class GetReportDefinitionHandler(MicrofinanceDbContext context) : IQueryHandler<GetReportDefinitionQuery, ReportDefinitionDto>
{
    public async ValueTask<ReportDefinitionDto> Handle(GetReportDefinitionQuery query, CancellationToken ct)
    {
        var entity = await context.ReportDefinitions
            .Where(x => x.Id == query.Id)
            .Select(x => new ReportDefinitionDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ReportDefinition not found");
    }
}
