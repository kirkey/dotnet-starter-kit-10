using Accounting.Application.Reports.WriteOffSummary.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

public sealed class WriteOffSummaryReportService : IWriteOffSummaryReportService
{
    public WriteOffSummaryReportService() => QuestPDF.Settings.License = LicenseType.Community;
    public async Task<byte[]> GenerateReportAsync() => await Task.Run(() => Document.Create(c => c.Page(p => { p.Size(PageSizes.A4); p.Margin(30); p.Header().Text("WRITE-OFF SUMMARY").FontSize(16).Bold(); p.Content().PaddingTop(20).Text("Summary of write-offs by period and category.").FontSize(10); p.Footer().AlignRight().Text(t => { t.Span("Page "); t.CurrentPageNumber(); }); })).GeneratePdf());
}
