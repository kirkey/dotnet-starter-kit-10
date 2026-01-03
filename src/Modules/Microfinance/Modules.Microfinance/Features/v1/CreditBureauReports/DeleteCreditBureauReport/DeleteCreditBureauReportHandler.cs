using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.CreditBureauReports.DeleteCreditBureauReport;

public record DeleteCreditBureauReportCommand(Guid Id) : ICommand;

public class DeleteCreditBureauReportHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteCreditBureauReportCommand>
{
    public async ValueTask<Unit> Handle(DeleteCreditBureauReportCommand command, CancellationToken ct)
    {
        var entity = await context.CreditBureauReports.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("CreditBureauReport not found");
        
        context.CreditBureauReports.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
