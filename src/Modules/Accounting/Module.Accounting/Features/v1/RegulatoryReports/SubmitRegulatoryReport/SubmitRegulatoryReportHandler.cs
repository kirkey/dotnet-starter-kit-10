// TODO: Implement Submit operation for RegulatoryReport
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.RegulatoryReports.SubmitRegulatoryReport;

namespace FSH.Module.Accounting.Features.v1.RegulatoryReports.SubmitRegulatoryReport;

public class SubmitRegulatoryReportHandler(AccountingDbContext context) 
    : ICommandHandler<SubmitRegulatoryReportCommand>
{
    public async ValueTask<Unit> Handle(SubmitRegulatoryReportCommand command, CancellationToken ct)
    {
        // TODO: Implement Submit logic
        throw new NotImplementedException("Submit operation for RegulatoryReport needs to be implemented");
    }
}
