using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.ReportDefinitions.UpdateReportDefinition;

public record UpdateReportDefinitionCommand(Guid Id, string Name) : ICommand<Guid>;

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
