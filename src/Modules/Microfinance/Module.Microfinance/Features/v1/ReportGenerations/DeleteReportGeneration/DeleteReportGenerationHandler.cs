using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.ReportGenerations.DeleteReportGeneration;

namespace FSH.Module.Microfinance.Features.v1.ReportGenerations.DeleteReportGeneration;

public class DeleteReportGenerationHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteReportGenerationCommand>
{
    public async ValueTask<Unit> Handle(DeleteReportGenerationCommand command, CancellationToken ct)
    {
        var entity = await context.ReportGenerations.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ReportGeneration not found");
        
        context.ReportGenerations.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
