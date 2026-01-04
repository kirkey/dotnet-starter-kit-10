// TODO: Implement Submit operation for RegulatoryReport
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RegulatoryReports.SubmitRegulatoryReport;

public record SubmitRegulatoryReportCommand(Guid Id) : ICommand;

public class SubmitRegulatoryReportHandler(AccountingDbContext context) 
    : ICommandHandler<SubmitRegulatoryReportCommand>
{
    public async ValueTask<Unit> Handle(SubmitRegulatoryReportCommand command, CancellationToken ct)
    {
        // TODO: Implement Submit logic
        throw new NotImplementedException("Submit operation for RegulatoryReport needs to be implemented");
    }
}
