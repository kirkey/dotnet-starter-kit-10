using Accounting.Application.Reports.BalanceSheet.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Balance Sheet PDF report.
/// </summary>
public static class GenerateAccountingBalanceSheetReportEndpoint
{
    /// <summary>
    /// Maps the Balance Sheet PDF report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingBalanceSheetReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/balance-sheet", async (
                [AsParameters] AccountingBalanceSheetReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var asOfDate = request.AsOfDate ?? DateTime.UtcNow;
                
                var command = new GenerateBalanceSheetReportCommand(
                    asOfDate, 
                    request.IncludeComparative,
                    request.ComparativeAsOfDate);
                    
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                var fileName = request.IncludeComparative && request.ComparativeAsOfDate.HasValue
                    ? $"BalanceSheet_{asOfDate:yyyyMMdd}_Comparative_{request.ComparativeAsOfDate.Value:yyyyMMdd}.pdf"
                    : $"BalanceSheet_{asOfDate:yyyyMMdd}.pdf";
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    fileName);
            })
            .WithName(nameof(GenerateAccountingBalanceSheetReportEndpoint))
            .WithSummary("Generate Balance Sheet PDF report")
            .WithDescription("Generates a PDF report showing the Balance Sheet as of a specific date. Optionally includes comparative period data.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Balance Sheet PDF report.
/// </summary>
public record AccountingBalanceSheetReportRequest(
    DateTime? AsOfDate = null,
    bool IncludeComparative = false,
    DateTime? ComparativeAsOfDate = null);
