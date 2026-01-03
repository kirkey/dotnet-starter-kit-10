using FSH.Framework.Core.Context;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.CreditBureauReports.CreateCreditBureauReport;

public record CreateCreditBureauReportCommand(string Name) : ICommand<Guid>;

public class CreateCreditBureauReportHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateCreditBureauReportCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateCreditBureauReportCommand command, CancellationToken ct)
    {
        var entity = CreditBureauReport.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.CreditBureauReports.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
