using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;
using Accounting.Application.Reports.BankReconciliation.v1.Services;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.BankReconciliations.ExportBankReconciliation;

public record ExportBankReconciliationCommand(Guid Id) : ICommand;

public class ExportBankReconciliationHandler(AccountingDbContext context, IBankReconciliationReportService reportService) 
    : ICommandHandler<ExportBankReconciliationCommand>
{
    public async ValueTask<Unit> Handle(ExportBankReconciliationCommand command, CancellationToken ct)
    {
        var entity = await context.BankReconciliations.FirstOrDefaultAsync(x => x.Id == command.Id, ct)
            ?? throw new NotFoundException("BankReconciliation not found");

        var _ = await reportService.GenerateReportAsync(command.Id);
        // TODO: return or persist exported bytes if needed

        return Unit.Value;
    }
}
