using Accounting.Application.Reports.Bill.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingBillReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingBillReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/bill/{billId:guid}", async (
                Guid billId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GenerateBillReportCommand(billId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"Bill_{billId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingBillReportEndpoint))
            .WithSummary("Generate Bill PDF report")
            .WithDescription("Generates a PDF report for an individual bill including all line items.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
