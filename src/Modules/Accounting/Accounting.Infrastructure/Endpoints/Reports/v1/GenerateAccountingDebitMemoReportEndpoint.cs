using Accounting.Application.Reports.DebitMemo.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingDebitMemoReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingDebitMemoReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/debit-memo/{debitMemoId:guid}", async (
                Guid debitMemoId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GenerateDebitMemoReportCommand(debitMemoId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"DebitMemo_{debitMemoId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingDebitMemoReportEndpoint))
            .WithSummary("Generate Debit Memo PDF report")
            .WithDescription("Generates a PDF report for an individual debit memo including all details.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
