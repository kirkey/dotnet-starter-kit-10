using Accounting.Application.Reports.DepreciationSchedule.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingDepreciationScheduleReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingDepreciationScheduleReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LDepreciation-LSchedule", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateDepreciationScheduleReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "DepreciationSchedule.pdf");
            })
            .WithName(nameof(GenerateAccountingDepreciationScheduleReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
