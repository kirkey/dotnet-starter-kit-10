using Accounting.Application.Reports.AgedReceivables.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Aged Receivables PDF report.
/// </summary>
public static class GenerateAccountingAgedReceivablesReportEndpoint
{
    /// <summary>
    /// Maps the Aged Receivables report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingAgedReceivablesReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/aged-receivables", async (
                [AsParameters] AccountingAgedReceivablesReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var asOfDate = request.AsOfDate ?? DateTime.UtcNow;
                
                var command = new GenerateAccountingAgedReceivablesReportCommand(
                    asOfDate, 
                    request.CustomerId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"AgedReceivables_{asOfDate:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingAgedReceivablesReportEndpoint))
            .WithSummary("Generate Aged Receivables PDF report")
            .WithDescription("Generates a PDF report showing all outstanding receivables with aging analysis.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Aged Receivables report.
/// </summary>
public record AccountingAgedReceivablesReportRequest(
    DateTime? AsOfDate = null, 
    Guid? CustomerId = null);
