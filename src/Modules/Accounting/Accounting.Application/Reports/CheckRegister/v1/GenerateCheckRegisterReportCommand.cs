using Accounting.Application.Reports.CheckRegister.v1.Services;

namespace Accounting.Application.Reports.CheckRegister.v1;

public record GenerateCheckRegisterReportCommand(DateTime StartDate, DateTime EndDate) : IRequest<byte[]>;

public sealed class GenerateCheckRegisterReportHandler : IRequestHandler<GenerateCheckRegisterReportCommand, byte[]>
{
    private readonly ICheckRegisterReportService _reportService;
    public GenerateCheckRegisterReportHandler(ICheckRegisterReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateCheckRegisterReportCommand request, CancellationToken cancellationToken) 
        => await _reportService.GenerateReportAsync(request.StartDate, request.EndDate);
}
