// TODO: Implement Generate operation for RegulatoryReport
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.RegulatoryReports.GenerateRegulatoryReport;

public record GenerateRegulatoryReportCommand(Guid Id) : ICommand;

public class GenerateRegulatoryReportHandler(AccountingDbContext context) 
    : ICommandHandler<GenerateRegulatoryReportCommand>
{
    public async ValueTask<Unit> Handle(GenerateRegulatoryReportCommand command, CancellationToken ct)
    {
        // TODO: Implement Generate logic
        throw new NotImplementedException("Generate operation for RegulatoryReport needs to be implemented");
    }
}
