using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.ReportGenerations;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ReportGenerations.GetReportGeneration;

public record GetReportGenerationQuery(Guid Id) : IQuery<ReportGenerationDto>;

public class GetReportGenerationHandler(MicrofinanceDbContext context) : IQueryHandler<GetReportGenerationQuery, ReportGenerationDto>
{
    public async ValueTask<ReportGenerationDto> Handle(GetReportGenerationQuery query, CancellationToken ct)
    {
        var entity = await context.ReportGenerations
            .Where(x => x.Id == query.Id)
            .Select(x => new ReportGenerationDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("ReportGeneration not found");
    }
}
