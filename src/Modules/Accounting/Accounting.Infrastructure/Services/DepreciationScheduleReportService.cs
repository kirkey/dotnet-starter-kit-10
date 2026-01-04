using Accounting.Application.Reports.DepreciationSchedule.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

public sealed class DepreciationScheduleReportService : IDepreciationScheduleReportService
{
    public DepreciationScheduleReportService() => QuestPDF.Settings.License = LicenseType.Community;
    public async Task<byte[]> GenerateReportAsync() => await Task.Run(() => Document.Create(c => c.Page(p => { p.Size(PageSizes.A4.Landscape()); p.Margin(30); p.Header().Text("DEPRECIATION SCHEDULE").FontSize(16).Bold(); p.Content().PaddingTop(20).Text("Depreciation schedule by asset and period.").FontSize(10); p.Footer().AlignRight().Text(t => { t.Span("Page "); t.CurrentPageNumber(); }); })).GeneratePdf());
}
