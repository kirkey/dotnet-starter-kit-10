using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ReportDefinitions.DeleteReportDefinition;

public record DeleteReportDefinitionCommand(Guid Id) : ICommand;

public class DeleteReportDefinitionHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteReportDefinitionCommand>
{
    public async ValueTask<Unit> Handle(DeleteReportDefinitionCommand command, CancellationToken ct)
    {
        var entity = await context.ReportDefinitions.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("ReportDefinition not found");
        
        context.ReportDefinitions.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
