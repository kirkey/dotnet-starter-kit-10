using Accounting.Application.Reports.WriteOffSummary.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingWriteOffSummaryReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingWriteOffSummaryReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LWrite-LOff-LSummary", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateWriteOffSummaryReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "WriteOffSummary.pdf");
            })
            .WithName(nameof(GenerateAccountingWriteOffSummaryReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
