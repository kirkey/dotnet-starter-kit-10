using Accounting.Application.Reports.CustomerStatement.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingCustomerStatementReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingCustomerStatementReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/customer-statement/{customerId:guid}", async (
                Guid customerId,
                DateTime? startDate,
                DateTime? endDate,
                ISender sender, 
                CancellationToken cancellationToken) =>
            {
                var start = startDate ?? DateTime.UtcNow.AddMonths(-1).Date;
                var end = endDate ?? DateTime.UtcNow.Date;
                
                var command = new GenerateCustomerStatementReportCommand(customerId, start, end);
                var pdfBytes = await sender.Send(command, cancellationToken);
                
                return Results.File(pdfBytes, "application/pdf", $"CustomerStatement_{customerId:N}_{start:yyyyMMdd}_{end:yyyyMMdd}.pdf");
            })
            .WithName(nameof(GenerateAccountingCustomerStatementReportEndpoint))
            .WithSummary("Generate Customer Statement PDF")
            .WithDescription("Generates a customer statement showing invoices, payments, and running balance for a period.")
            .Produces<FileResult>()
            .MapToApiVersion(1);

        return app;
    }
}
