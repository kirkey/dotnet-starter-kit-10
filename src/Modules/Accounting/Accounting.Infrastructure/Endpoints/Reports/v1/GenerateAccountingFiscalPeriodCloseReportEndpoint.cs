using Accounting.Application.Reports.FiscalPeriodClose.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingFiscalPeriodCloseReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingFiscalPeriodCloseReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LFiscal-LPeriod-LClose", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateFiscalPeriodCloseReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "FiscalPeriodClose.pdf");
            })
            .WithName(nameof(GenerateAccountingFiscalPeriodCloseReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
