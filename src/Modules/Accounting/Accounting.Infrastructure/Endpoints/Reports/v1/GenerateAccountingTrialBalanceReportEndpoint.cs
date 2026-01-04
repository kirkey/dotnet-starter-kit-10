using Accounting.Application.Reports.TrialBalance.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Trial Balance PDF report.
/// </summary>
public static class GenerateAccountingTrialBalanceReportEndpoint
{
    /// <summary>
    /// Maps the Trial Balance report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingTrialBalanceReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/trial-balance", async (
                [AsParameters] AccountingTrialBalanceReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var asOfDate = request.AsOfDate ?? DateTime.UtcNow;
                
                var command = new GenerateAccountingTrialBalanceReportCommand(
                    asOfDate, 
                    request.PeriodId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"TrialBalance_{asOfDate:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingTrialBalanceReportEndpoint))
            .WithSummary("Generate Trial Balance PDF report")
            .WithDescription("Generates a PDF report showing the Trial Balance as of the specified date.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Trial Balance report.
/// </summary>
public record AccountingTrialBalanceReportRequest(
    DateTime? AsOfDate = null, 
    Guid? PeriodId = null);
