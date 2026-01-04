using Accounting.Application.Reports.VendorStatement.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingVendorStatementReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingVendorStatementReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/vendor-statement/{vendorId:guid}", async (
                Guid vendorId,
                DateTime? startDate,
                DateTime? endDate,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var start = startDate ?? DateTime.UtcNow.AddMonths(-1).Date;
                var end = endDate ?? DateTime.UtcNow.Date;
                
                var command = new GenerateVendorStatementReportCommand(vendorId, start, end);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"VendorStatement_{vendorId:N}_{start:yyyyMMdd}_{end:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingVendorStatementReportEndpoint))
            .WithSummary("Generate Vendor Statement PDF")
            .WithDescription("Generates a vendor statement showing bills and outstanding amounts for a period.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
