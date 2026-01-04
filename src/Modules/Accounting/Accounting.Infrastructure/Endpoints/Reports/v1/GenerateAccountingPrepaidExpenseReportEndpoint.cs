using Accounting.Application.Reports.PrepaidExpense.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingPrepaidExpenseReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingPrepaidExpenseReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LPrepaid-LExpense", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GeneratePrepaidExpenseReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "PrepaidExpense.pdf");
            })
            .WithName(nameof(GenerateAccountingPrepaidExpenseReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
