using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ReportDefinitions.UpdateReportDefinition;

namespace FSH.Module.Microfinance.Features.v1.ReportDefinitions.UpdateReportDefinition;

public class UpdateReportDefinitionHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateReportDefinitionCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateReportDefinitionCommand command, CancellationToken ct)
    {
        var entity = await context.ReportDefinitions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ReportDefinition not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
