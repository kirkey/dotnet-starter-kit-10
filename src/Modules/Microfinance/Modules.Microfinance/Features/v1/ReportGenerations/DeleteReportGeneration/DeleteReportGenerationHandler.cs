using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ReportGenerations.DeleteReportGeneration;

public record DeleteReportGenerationCommand(Guid Id) : ICommand;

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
