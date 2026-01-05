using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.DeleteCreditBureauReport;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.DeleteCreditBureauReport;

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
