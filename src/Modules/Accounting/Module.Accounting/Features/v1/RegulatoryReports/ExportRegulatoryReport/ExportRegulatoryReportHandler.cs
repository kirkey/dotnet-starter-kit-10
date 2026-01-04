// TODO: Implement Export operation for RegulatoryReport
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.RegulatoryReports.ExportRegulatoryReport;

public record ExportRegulatoryReportCommand(Guid Id) : ICommand;

public class ExportRegulatoryReportHandler(AccountingDbContext context) 
    : ICommandHandler<ExportRegulatoryReportCommand>
{
    public async ValueTask<Unit> Handle(ExportRegulatoryReportCommand command, CancellationToken ct)
    {
        // TODO: Implement Export logic
        throw new NotImplementedException("Export operation for RegulatoryReport needs to be implemented");
    }
}
