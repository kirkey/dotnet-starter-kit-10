using Accounting.Application.Reports.CheckRegister.v1;

namespace Accounting.Infrastructure.Endpoints.Reports.v1;

public static class GenerateAccountingCheckRegisterReportEndpoint
{
    internal static IEndpointRouteBuilder MapGenerateAccountingCheckRegisterReportEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/report/check-register", async (DateTime? startDate, DateTime? endDate, ISender sender, CancellationToken ct) =>
            {
                var start = startDate ?? DateTime.UtcNow.AddMonths(-1).Date;
                var end = endDate ?? DateTime.UtcNow.Date;
                var command = new GenerateCheckRegisterReportCommand(start, end);
                var pdfBytes = await sender.Send(command, ct);
                return Results.File(pdfBytes, "application/pdf", "CheckRegister.pdf");
            })
            .WithName(nameof(GenerateAccountingCheckRegisterReportEndpoint))
            .MapToApiVersion(1);
        return app;
    }
}
