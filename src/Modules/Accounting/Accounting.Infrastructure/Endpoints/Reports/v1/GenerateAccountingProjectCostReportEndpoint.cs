using Accounting.Application.Reports.ProjectCost.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingProjectCostReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingProjectCostReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LProject-LCost", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateProjectCostReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "ProjectCost.pdf");
            })
            .WithName(nameof(GenerateAccountingProjectCostReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
