using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CreditBureauReports.UpdateCreditBureauReport;

public record UpdateCreditBureauReportCommand(Guid Id, string Name) : ICommand<Guid>;

public class UpdateCreditBureauReportHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateCreditBureauReportCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCreditBureauReportCommand command, CancellationToken ct)
    {
        var entity = await context.CreditBureauReports.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CreditBureauReport not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
