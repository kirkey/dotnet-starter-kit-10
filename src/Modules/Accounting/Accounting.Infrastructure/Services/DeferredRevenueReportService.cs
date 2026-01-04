using Accounting.Application.Reports.DeferredRevenue.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

public sealed class DeferredRevenueReportService : IDeferredRevenueReportService
{
    public DeferredRevenueReportService() => QuestPDF.Settings.License = LicenseType.Community;
    public async Task<byte[]> GenerateReportAsync() => await Task.Run(() => Document.Create(c => c.Page(p => { p.Size(PageSizes.A4); p.Margin(30); p.Header().Text("DEFERRED REVENUE SCHEDULE").FontSize(16).Bold(); p.Content().PaddingTop(20).Text("Deferred revenue recognition schedule.").FontSize(10); p.Footer().AlignRight().Text(t => { t.Span("Page "); t.CurrentPageNumber(); }); })).GeneratePdf());
}
