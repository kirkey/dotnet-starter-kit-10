using Accounting.Application.Reports.FixedAssetRegister.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingFixedAssetRegisterReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingFixedAssetRegisterReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LFixed-LAsset-LRegister", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateFixedAssetRegisterReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "FixedAssetRegister.pdf");
            })
            .WithName(nameof(GenerateAccountingFixedAssetRegisterReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
