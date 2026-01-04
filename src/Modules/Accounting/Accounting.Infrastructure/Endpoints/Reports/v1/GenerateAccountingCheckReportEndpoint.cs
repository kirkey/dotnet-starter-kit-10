using Accounting.Application.Reports.Check.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingCheckReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingCheckReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/check/{checkId:guid}", async (
                Guid checkId,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var command = new GenerateCheckReportCommand(checkId);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"Check_{checkId:N}.pdf");
            })
            .WithName(nameof(GenerateAccountingCheckReportEndpoint))
            .WithSummary("Generate Printable Check PDF")
            .WithDescription("Generates a printable check PDF with MICR line and check stub.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
