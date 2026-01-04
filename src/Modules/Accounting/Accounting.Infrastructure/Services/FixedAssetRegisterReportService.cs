using Accounting.Application.Reports.FixedAssetRegister.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

public sealed class FixedAssetRegisterReportService : IFixedAssetRegisterReportService
{
    public FixedAssetRegisterReportService() => QuestPDF.Settings.License = LicenseType.Community;

    public async Task<byte[]> GenerateReportAsync()
    {
        return await Task.Run(() => Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(30);
                page.Header().Text("FIXED ASSET REGISTER").FontSize(16).Bold().FontColor(Colors.Blue.Darken2);
                page.Content().PaddingTop(20).Column(col =>
                {
                    col.Item().Text("Asset register showing all fixed assets, cost, accumulated depreciation, and net book value.").FontSize(10);
                    col.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(c => { c.RelativeColumn(); c.ConstantColumn(80); c.ConstantColumn(80); c.ConstantColumn(80); c.ConstantColumn(80); });
                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Asset").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight().Text("Cost").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight().Text("Accum Depr").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight().Text("Net Book").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Status").FontSize(9).SemiBold();
                        });
                        table.Cell().Padding(3).Text("Sample Asset Data").FontSize(8);
                        table.Cell().Padding(3).AlignRight().Text("$0.00").FontSize(8);
                        table.Cell().Padding(3).AlignRight().Text("$0.00").FontSize(8);
                        table.Cell().Padding(3).AlignRight().Text("$0.00").FontSize(8);
                        table.Cell().Padding(3).Text("Active").FontSize(8);
                    });
                });
                page.Footer().AlignRight().Text(p => { p.Span("Page "); p.CurrentPageNumber(); });
            });
        }).GeneratePdf());
    }
}
