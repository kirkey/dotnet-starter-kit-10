using Accounting.Application.Reports.PeriodStatus.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingPeriodStatusReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingPeriodStatusReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LPeriod-LStatus", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GeneratePeriodStatusReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "PeriodStatus.pdf");
            })
            .WithName(nameof(GenerateAccountingPeriodStatusReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
