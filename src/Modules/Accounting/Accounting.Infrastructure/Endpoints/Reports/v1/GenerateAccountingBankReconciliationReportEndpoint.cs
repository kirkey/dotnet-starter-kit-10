using Accounting.Application.Reports.BankReconciliation.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingBankReconciliationReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingBankReconciliationReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/bank-reconciliation/{reconciliationId:guid}", async (
                Guid reconciliationId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GenerateBankReconciliationReportCommand(reconciliationId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"BankReconciliation_{reconciliationId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingBankReconciliationReportEndpoint))
            .WithSummary("Generate Bank Reconciliation PDF")
            .WithDescription("Generates a bank reconciliation report showing statement vs book balances.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
