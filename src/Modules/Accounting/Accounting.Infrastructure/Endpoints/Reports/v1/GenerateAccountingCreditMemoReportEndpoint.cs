using Accounting.Application.Reports.CreditMemo.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating individual Credit Memo PDF report.
/// </summary>
public static class GenerateAccountingCreditMemoReportEndpoint
{
    /// <summary>
    /// Maps the Credit Memo PDF report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingCreditMemoReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/credit-memo/{creditMemoId:guid}", async (
                Guid creditMemoId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GenerateCreditMemoReportCommand(creditMemoId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"CreditMemo_{creditMemoId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingCreditMemoReportEndpoint))
            .WithSummary("Generate Credit Memo PDF report")
            .WithDescription("Generates a PDF report for an individual credit memo including all details and application status.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
