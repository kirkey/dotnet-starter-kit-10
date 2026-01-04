using Accounting.Application.Reports.Payment.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingPaymentReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingPaymentReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/payment/{paymentId:guid}", async (
                Guid paymentId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GeneratePaymentReportCommand(paymentId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"PaymentReceipt_{paymentId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingPaymentReportEndpoint))
            .WithSummary("Generate Payment Receipt PDF")
            .WithDescription("Generates a PDF receipt for a payment including allocation details.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
