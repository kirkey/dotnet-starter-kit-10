using Accounting.Application.Reports.CashFlowStatement.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating Cash Flow Statement PDF report.
/// </summary>
public static class GenerateAccountingCashFlowStatementReportEndpoint
{
    /// <summary>
    /// Maps the Cash Flow Statement PDF report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingCashFlowStatementReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/cash-flow-statement", async (
                [AsParameters] AccountingCashFlowStatementReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1).Date;
                var endDate = request.EndDate ?? DateTime.UtcNow.Date;
                
                var command = new GenerateCashFlowStatementReportCommand(
                    startDate, 
                    endDate,
                    request.IncludeComparative,
                    request.ComparativeStartDate,
                    request.ComparativeEndDate);
                    
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                var fileName = request.IncludeComparative && request.ComparativeStartDate.HasValue
                    ? $"CashFlowStatement_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}_Comparative.pdf"
                    : $"CashFlowStatement_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf";
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    fileName);
            })
            .WithName(nameof(GenerateAccountingCashFlowStatementReportEndpoint))
            .WithSummary("Generate Cash Flow Statement PDF report")
            .WithDescription("Generates a PDF report showing the Cash Flow Statement for a specific period. Optionally includes comparative period data.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for Cash Flow Statement PDF report.
/// </summary>
public record AccountingCashFlowStatementReportRequest(
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    bool IncludeComparative = false,
    DateTime? ComparativeStartDate = null,
    DateTime? ComparativeEndDate = null);
