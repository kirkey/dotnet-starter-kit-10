using Accounting.Application.Reports.JournalEntry.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Journal Entry PDF report.
/// </summary>
public static class GenerateAccountingJournalEntryReportEndpoint
{
    /// <summary>
    /// Maps the Journal Entry report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingJournalEntryReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/journal-entries", async (
                [AsParameters] AccountingJournalEntryReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1);
                var endDate = request.EndDate ?? DateTime.UtcNow;
                
                var command = new GenerateAccountingJournalEntryReportCommand(
                    startDate, 
                    endDate, 
                    request.IsPostedOnly ?? false);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"JournalEntries_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingJournalEntryReportEndpoint))
            .WithSummary("Generate Journal Entry PDF report")
            .WithDescription("Generates a PDF report showing all Journal Entries for the specified period.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Journal Entry report.
/// </summary>
public record AccountingJournalEntryReportRequest(
    DateTime? StartDate = null, 
    DateTime? EndDate = null, 
    bool? IsPostedOnly = false);
