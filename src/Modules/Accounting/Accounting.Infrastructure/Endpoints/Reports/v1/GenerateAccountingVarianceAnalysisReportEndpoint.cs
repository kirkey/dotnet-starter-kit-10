using Accounting.Application.Reports.VarianceAnalysis.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingVarianceAnalysisReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingVarianceAnalysisReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LVariance-LAnalysis", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateVarianceAnalysisReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "VarianceAnalysis.pdf");
            })
            .WithName(nameof(GenerateAccountingVarianceAnalysisReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
