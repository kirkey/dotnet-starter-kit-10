using Accounting.Application.Reports.AgedPayables.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Aged Payables PDF report.
/// </summary>
public static class GenerateAccountingAgedPayablesReportEndpoint
{
    /// <summary>
    /// Maps the Aged Payables report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingAgedPayablesReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/aged-payables", async (
                [AsParameters] AccountingAgedPayablesReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var asOfDate = request.AsOfDate ?? DateTime.UtcNow;
                
                var command = new GenerateAccountingAgedPayablesReportCommand(
                    asOfDate, 
                    request.VendorId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"AgedPayables_{asOfDate:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingAgedPayablesReportEndpoint))
            .WithSummary("Generate Aged Payables PDF report")
            .WithDescription("Generates a PDF report showing all outstanding payables with aging analysis.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Aged Payables report.
/// </summary>
public record AccountingAgedPayablesReportRequest(
    DateTime? AsOfDate = null, 
    Guid? VendorId = null);
