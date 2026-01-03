using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.ReportDefinitions.CreateReportDefinition;

public record CreateReportDefinitionCommand(string Name) : ICommand<Guid>;

public class CreateReportDefinitionHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateReportDefinitionCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateReportDefinitionCommand command, CancellationToken ct)
    {
        var entity = ReportDefinition.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ReportDefinitions.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
