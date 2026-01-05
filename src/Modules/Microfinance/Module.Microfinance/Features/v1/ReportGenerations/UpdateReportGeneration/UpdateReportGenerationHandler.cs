using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ReportGenerations.UpdateReportGeneration;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.UpdateReportGeneration;

public class UpdateReportGenerationHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateReportGenerationCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateReportGenerationCommand command, CancellationToken ct)
    {
        var entity = await context.ReportGenerations.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ReportGeneration not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
