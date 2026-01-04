using Accounting.Application.Reports.FixedAssetRegister.v1.Services;

namespace Accounting.Application.Reports.FixedAssetRegister.v1;

public record GenerateFixedAssetRegisterReportCommand() : IRequest<byte[]>;

public sealed class GenerateFixedAssetRegisterReportHandler : IRequestHandler<GenerateFixedAssetRegisterReportCommand, byte[]>
{
    private readonly IFixedAssetRegisterReportService _reportService;
    public GenerateFixedAssetRegisterReportHandler(IFixedAssetRegisterReportService reportService) => _reportService = reportService;
    public async Task<byte[]> Handle(GenerateFixedAssetRegisterReportCommand request, CancellationToken ct) 
        => await _reportService.GenerateReportAsync();
}
