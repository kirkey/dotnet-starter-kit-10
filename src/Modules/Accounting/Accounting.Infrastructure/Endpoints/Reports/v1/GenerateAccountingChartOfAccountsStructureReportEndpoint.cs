using Accounting.Application.Reports.ChartOfAccountsStructure.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingChartOfAccountsStructureReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingChartOfAccountsStructureReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LChart-LOf-LAccounts-LStructure", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateChartOfAccountsStructureReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "ChartOfAccountsStructure.pdf");
            })
            .WithName(nameof(GenerateAccountingChartOfAccountsStructureReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
