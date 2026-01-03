using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.ReportGenerations.CreateReportGeneration;

public record CreateReportGenerationCommand(string Name) : ICommand<Guid>;

public class CreateReportGenerationHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateReportGenerationCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateReportGenerationCommand command, CancellationToken ct)
    {
        var entity = ReportGeneration.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ReportGenerations.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
