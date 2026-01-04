using Accounting.Application.Reports.DeferredRevenue.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingDeferredRevenueReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingDeferredRevenueReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LDeferred-LRevenue", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateDeferredRevenueReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "DeferredRevenue.pdf");
            })
            .WithName(nameof(GenerateAccountingDeferredRevenueReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
