using Accounting.Application.Reports.BudgetVsActual.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingBudgetVsActualReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingBudgetVsActualReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LBudget-LVs-LActual", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateBudgetVsActualReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "BudgetVsActual.pdf");
            })
            .WithName(nameof(GenerateAccountingBudgetVsActualReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
