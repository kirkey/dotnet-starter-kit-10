using Accounting.Application.Reports.PrepaidExpense.v1.Services;

namespace Accounting.Application.Reports.PrepaidExpense.v1;

public record GeneratePrepaidExpenseReportCommand() : IRequest<byte[]>;

public sealed class GeneratePrepaidExpenseReportHandler : IRequestHandler<GeneratePrepaidExpenseReportCommand, byte[]>
{
    private readonly IPrepaidExpenseReportService _reportService;
    public GeneratePrepaidExpenseReportHandler(IPrepaidExpenseReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GeneratePrepaidExpenseReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
