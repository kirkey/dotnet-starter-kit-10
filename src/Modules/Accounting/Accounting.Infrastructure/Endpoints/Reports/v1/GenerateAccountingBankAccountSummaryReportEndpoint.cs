using Accounting.Application.Reports.BankAccountSummary.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingBankAccountSummaryReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingBankAccountSummaryReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/LBank-LAccount-LSummary", async (ISender sender, CancellationToken ct) =>
            {
                var command = new GenerateBankAccountSummaryReportCommand();
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "BankAccountSummary.pdf");
            })
            .WithName(nameof(GenerateAccountingBankAccountSummaryReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
