using Accounting.Application.Reports.Invoice.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating individual Invoice PDF report.
/// </summary>
public static class GenerateAccountingInvoiceReportEndpoint
{
    /// <summary>
    /// Maps the Invoice PDF report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingInvoiceReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/invoice/{invoiceId:guid}", async (
                Guid invoiceId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GenerateInvoiceReportCommand(invoiceId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"Invoice_{invoiceId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingInvoiceReportEndpoint))
            .WithSummary("Generate Invoice PDF report")
            .WithDescription("Generates a PDF report for an individual invoice including all line items and charges.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
