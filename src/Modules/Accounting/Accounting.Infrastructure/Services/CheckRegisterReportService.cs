using Accounting.Application.Reports.CheckRegister.v1.Services;
using Ardalis.Specification;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

public sealed class CheckRegisterReportService : ICheckRegisterReportService
{
    private readonly IReadRepository<Check> _checkRepository;
    public CheckRegisterReportService([FromKeyedServices("accounting:checks")] IReadRepository<Check> checkRepository)
    {
        _checkRepository = checkRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateReportAsync(DateTime startDate, DateTime endDate)
    {
        var spec = new ChecksForPeriodSpec(startDate, endDate);
        var checks = await _checkRepository.ListAsync(spec);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(9));
                page.Header().Element(c => ComposeHeader(c, startDate, endDate));
                page.Content().Element(c => ComposeContent(c, checks.ToList()));
                page.Footer().Element(ComposeFooter);
            });
        });
        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, DateTime startDate, DateTime endDate)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Text("CHECK REGISTER").FontSize(16).Bold().FontColor(Colors.Blue.Darken2);
                row.RelativeItem().AlignRight().Text($"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}").FontSize(10);
            });
            col.Item().PaddingTop(10).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, List<Check> checks)
    {
        container.PaddingTop(15).Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(70);  // Date
                columns.ConstantColumn(90);  // Check #
                columns.RelativeColumn(2);   // Payee
                columns.RelativeColumn(2);   // Memo
                columns.ConstantColumn(80);  // Amount
                columns.ConstantColumn(70);  // Status
            });

            table.Header(header =>
            {
                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Date").FontSize(9).SemiBold();
                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Check #").FontSize(9).SemiBold();
                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Payee").FontSize(9).SemiBold();
                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Memo").FontSize(9).SemiBold();
                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight().Text("Amount").FontSize(9).SemiBold();
                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Status").FontSize(9).SemiBold();
            });

            var isAlternate = false;
            var total = 0m;
            foreach (var check in checks.OrderBy(c => c.IssuedDate))
            {
                var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;
                table.Cell().Background(bgColor).Padding(3).Text(check.IssuedDate?.ToString("MM/dd/yyyy") ?? "-").FontSize(8);
                table.Cell().Background(bgColor).Padding(3).Text(check.CheckNumber ?? "-").FontSize(8);
                table.Cell().Background(bgColor).Padding(3).Text($"Vendor {check.VendorId}").FontSize(8);
                table.Cell().Background(bgColor).Padding(3).Text(check.Memo ?? "-").FontSize(8);
                table.Cell().Background(bgColor).Padding(3).AlignRight().Text((check.Amount ?? 0m).ToString("N2")).FontSize(8);
                table.Cell().Background(bgColor).Padding(3).Text(check.Status).FontSize(8);
                total += check.Amount ?? 0m;
                isAlternate = !isAlternate;
            }

            table.Cell().ColumnSpan(4).Background(Colors.Grey.Lighten2).Padding(4).Text("TOTAL").FontSize(9).Bold().AlignRight();
            table.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight().Text(total.ToString("N2")).FontSize(9).Bold();
            table.Cell().Background(Colors.Grey.Lighten2).Padding(4);
        });
    }

    private static void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Text("Check Register Report").FontSize(7);
            row.RelativeItem().AlignRight().Text($"Generated: {DateTime.Now:MMM dd, yyyy}").FontSize(7);
        });
    }
}

internal sealed class ChecksForPeriodSpec : Specification<Check>
{
    public ChecksForPeriodSpec(DateTime startDate, DateTime endDate)
    {
        Query.Where(c => c.IssuedDate >= startDate && c.IssuedDate <= endDate);
        Query.OrderBy(c => c.IssuedDate);
    }
}
