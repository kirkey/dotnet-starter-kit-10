using Accounting.Application.Reports.GeneralLedger.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

/// <summary>
/// Endpoint for generating General Ledger PDF report.
/// </summary>
public static class GenerateAccountingGeneralLedgerReportEndpoint
{
    /// <summary>
    /// Maps the General Ledger report endpoint.
    /// </summary>
    internal static IEndpointRouteBuilder MapGenerateAccountingGeneralLedgerReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/general-ledger", async (
                [AsParameters] AccountingGeneralLedgerReportRequest request,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var startDate = request.StartDate ?? DateTime.UtcNow.AddMonths(-1);
                var endDate = request.EndDate ?? DateTime.UtcNow;
                
                var command = new GenerateAccountingGeneralLedgerReportCommand(
                    startDate, 
                    endDate, 
                    request.AccountId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(
                    pdfBytes,
                    "application/pdf",
                    $"GeneralLedger_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingGeneralLedgerReportEndpoint))
            .WithSummary("Generate General Ledger PDF report")
            .WithDescription("Generates a PDF report showing all General Ledger entries for the specified period.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}

/// <summary>
/// Request parameters for General Ledger report.
/// </summary>
public record AccountingGeneralLedgerReportRequest(
    DateTime? StartDate = null, 
    DateTime? EndDate = null, 
    Guid? AccountId = null);
