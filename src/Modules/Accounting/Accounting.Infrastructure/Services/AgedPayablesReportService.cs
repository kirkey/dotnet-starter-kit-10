using Accounting.Application.Reports.AgedPayables.v1.Services;
using Ardalis.Specification;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Aged Payables PDF reports using QuestPDF.
/// </summary>
public sealed class AgedPayablesReportService : IAgedPayablesReportService
{
    private readonly IReadRepository<Bill> _billRepository;
    private readonly IReadRepository<Vendor> _vendorRepository;

    public AgedPayablesReportService(
        IReadRepository<Bill> billRepository,
        IReadRepository<Vendor> vendorRepository)
    {
        _billRepository = billRepository;
        _vendorRepository = vendorRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DateTime asOfDate, Guid? vendorId = null)
    {
        var spec = new UnpaidBillsForReportSpec(vendorId);
        var bills = await _billRepository.ListAsync(spec);
        var vendors = await _vendorRepository.ListAsync();
        var vendorDict = vendors.ToDictionary(v => v.Id, v => v.Name);

        string? vendorName = null;
        if (vendorId.HasValue && vendorDict.TryGetValue(vendorId.Value, out var name))
        {
            vendorName = name;
        }

        // Calculate aging buckets
        var agingData = bills.Select(bill => new AgingItem
        {
            VendorName = vendorDict.TryGetValue(bill.VendorId, out var vName) ? vName : "Unknown",
            BillNumber = bill.BillNumber,
            BillDate = bill.BillDate,
            DueDate = bill.DueDate,
            TotalAmount = bill.TotalAmount,
            PaidAmount = bill.IsPaid ? bill.TotalAmount : 0m, // Bill doesn't have PaidAmount, use IsPaid flag
            OutstandingBalance = bill.IsPaid ? 0m : bill.TotalAmount,
            DaysOverdue = (asOfDate - bill.DueDate).Days,
            AgingBucket = GetAgingBucket((asOfDate - bill.DueDate).Days)
        }).Where(a => a.OutstandingBalance > 0).ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, asOfDate, vendorName));
                page.Content().Element(c => ComposeContent(c, agingData));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static string GetAgingBucket(int daysOverdue)
    {
        return daysOverdue switch
        {
            <= 0 => "Current",
            <= 30 => "1-30 Days",
            <= 60 => "31-60 Days",
            <= 90 => "61-90 Days",
            _ => "Over 90 Days"
        };
    }

    private static void ComposeHeader(IContainer container, DateTime asOfDate, string? vendorName)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("AGED PAYABLES REPORT")
                        .FontSize(20).Bold().FontColor(Colors.Red.Darken2);
                    col.Item().PaddingTop(5).Text("Accounting Module - Accounts Payable").FontSize(12).SemiBold();
                    col.Item().Text($"As Of: {asOfDate:MMMM dd, yyyy}").FontSize(10).SemiBold();
                    col.Item().Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    if (!string.IsNullOrEmpty(vendorName))
                        col.Item().Text($"Vendor: {vendorName}").FontSize(9).SemiBold();
                    else
                        col.Item().Text("All Vendors").FontSize(9).SemiBold();
                });
            });

            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium);
        });
    }

    private static void ComposeContent(IContainer container, List<AgingItem> items)
    {
        container.PaddingTop(15).Column(column =>
        {
            // Aging Summary section
            column.Item().Row(row =>
            {
                var current = items.Where(i => i.AgingBucket == "Current").Sum(i => i.OutstandingBalance);
                var days1To30 = items.Where(i => i.AgingBucket == "1-30 Days").Sum(i => i.OutstandingBalance);
                var days31To60 = items.Where(i => i.AgingBucket == "31-60 Days").Sum(i => i.OutstandingBalance);
                var days61To90 = items.Where(i => i.AgingBucket == "61-90 Days").Sum(i => i.OutstandingBalance);
                var over90 = items.Where(i => i.AgingBucket == "Over 90 Days").Sum(i => i.OutstandingBalance);
                var total = items.Sum(i => i.OutstandingBalance);

                row.RelativeItem().Background(Colors.Grey.Lighten4).Padding(10).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Text("Current").FontSize(9).SemiBold();
                        header.Cell().Text("1-30 Days").FontSize(9).SemiBold();
                        header.Cell().Text("31-60 Days").FontSize(9).SemiBold();
                        header.Cell().Text("61-90 Days").FontSize(9).SemiBold();
                        header.Cell().Text("Over 90 Days").FontSize(9).SemiBold();
                        header.Cell().Text("Total").FontSize(9).SemiBold();
                    });

                    table.Cell().Text(current.ToString("N2")).FontSize(10).FontColor(Colors.Green.Darken1);
                    table.Cell().Text(days1To30.ToString("N2")).FontSize(10).FontColor(Colors.Blue.Darken1);
                    table.Cell().Text(days31To60.ToString("N2")).FontSize(10).FontColor(Colors.Orange.Darken1);
                    table.Cell().Text(days61To90.ToString("N2")).FontSize(10).FontColor(Colors.Orange.Darken2);
                    table.Cell().Text(over90.ToString("N2")).FontSize(10).FontColor(Colors.Red.Darken1);
                    table.Cell().Text(total.ToString("N2")).FontSize(10).SemiBold();
                });
            });

            column.Item().PaddingTop(15);

            // Detail Table
            column.Item().Table(table =>
            {
                // Column definitions
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2);    // Vendor
                    columns.ConstantColumn(80);   // Bill #
                    columns.ConstantColumn(80);   // Bill Date
                    columns.ConstantColumn(80);   // Due Date
                    columns.ConstantColumn(80);   // Total
                    columns.ConstantColumn(80);   // Paid
                    columns.ConstantColumn(80);   // Outstanding
                    columns.ConstantColumn(60);   // Days
                    columns.ConstantColumn(80);   // Bucket
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Vendor").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Bill #").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Bill Date").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Due Date").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Total").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Paid").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Outstanding").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Days").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Red.Darken2).Padding(5)
                        .Text("Aging").FontColor(Colors.White).FontSize(9).SemiBold();
                });

                // Data rows
                var isAlternate = false;
                foreach (var item in items.OrderBy(i => i.VendorName).ThenByDescending(i => i.DaysOverdue))
                {
                    var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.VendorName).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.BillNumber).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.BillDate.ToString("MM/dd/yyyy")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.DueDate.ToString("MM/dd/yyyy")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(item.TotalAmount.ToString("N2")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(item.PaidAmount.ToString("N2")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(item.OutstandingBalance.ToString("N2")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(item.DaysOverdue > 0 ? item.DaysOverdue.ToString() : "-").FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.AgingBucket).FontSize(8);

                    isAlternate = !isAlternate;
                }

                // Totals row
                table.Cell().ColumnSpan(4).Background(Colors.Grey.Lighten2).Padding(5)
                    .Text("TOTAL").FontSize(9).SemiBold().AlignRight();
                table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                    .Text(items.Sum(i => i.TotalAmount).ToString("N2")).FontSize(9).SemiBold();
                table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                    .Text(items.Sum(i => i.PaidAmount).ToString("N2")).FontSize(9).SemiBold();
                table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                    .Text(items.Sum(i => i.OutstandingBalance).ToString("N2")).FontSize(9).SemiBold();
                table.Cell().ColumnSpan(2).Background(Colors.Grey.Lighten2).Padding(5).Text("");
            });
        });
    }

    private static void ComposeFooter(IContainer container)
    {
        container.BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(5).Row(row =>
        {
            row.RelativeItem().Text($"Generated by Accounting Module - {DateTime.Now:yyyy}")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
            row.RelativeItem().AlignRight().Text(text =>
            {
                text.Span("Page ").FontSize(8).FontColor(Colors.Grey.Darken1);
                text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Darken1);
                text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Darken1);
                text.TotalPages().FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }

    private class AgingItem
    {
        public string VendorName { get; set; } = string.Empty;
        public string BillNumber { get; set; } = string.Empty;
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingBalance { get; set; }
        public int DaysOverdue { get; set; }
        public string AgingBucket { get; set; } = string.Empty;
    }
}

/// <summary>
/// Specification for filtering unpaid bills for aging reports.
/// </summary>
internal sealed class UnpaidBillsForReportSpec : Specification<Bill>
{
    public UnpaidBillsForReportSpec(Guid? vendorId = null)
    {
        // Filter bills that are not paid
        Query.Where(bill => !bill.IsPaid);

        if (vendorId.HasValue)
        {
            Query.Where(bill => bill.VendorId == vendorId.Value);
        }

        Query.OrderBy(bill => bill.VendorId).ThenBy(bill => bill.DueDate);
    }
}
