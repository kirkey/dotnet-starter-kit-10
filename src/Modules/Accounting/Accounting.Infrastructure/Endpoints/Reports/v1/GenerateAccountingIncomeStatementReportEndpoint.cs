using Accounting.Application.Reports.IncomeStatement.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Income Statement PDF report.
/// </summary>
public static class GenerateAccountingIncomeStatementReportEndpoint
{
    /// <summary>
    /// Maps the Income Statement PDF report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingIncomeStatementReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/income-statement", async (
                [AsParameters] AccountingIncomeStatementReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1).Date;
                var endDate = request.EndDate ?? DateTime.UtcNow.Date;
                
                var command = new GenerateIncomeStatementReportCommand(
                    startDate, 
                    endDate,
                    request.IncludeComparative,
                    request.ComparativeStartDate,
                    request.ComparativeEndDate);
                    
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                var fileName = request.IncludeComparative && request.ComparativeStartDate.HasValue
                    ? $"IncomeStatement_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}_Comparative.pdf"
                    : $"IncomeStatement_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf";
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    fileName);
            })
            .WithName(nameof(GenerateAccountingIncomeStatementReportEndpoint))
            .WithSummary("Generate Income Statement PDF report")
            .WithDescription("Generates a PDF report showing the Income Statement for a specific period. Optionally includes comparative period data.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Income Statement PDF report.
/// </summary>
public record AccountingIncomeStatementReportRequest(
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    bool IncludeComparative = false,
    DateTime? ComparativeStartDate = null,
    DateTime? ComparativeEndDate = null);
