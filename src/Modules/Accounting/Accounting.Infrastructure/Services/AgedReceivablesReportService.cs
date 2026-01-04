using Accounting.Application.Reports.AgedReceivables.v1.Services;
using Ardalis.Specification;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Aged Receivables PDF reports using QuestPDF.
/// </summary>
public sealed class AgedReceivablesReportService : IAgedReceivablesReportService
{
    private readonly IReadRepository<Invoice> _invoiceRepository;
    private readonly IReadRepository<Member> _memberRepository;

    public AgedReceivablesReportService(
        IReadRepository<Invoice> invoiceRepository,
        IReadRepository<Member> memberRepository)
    {
        _invoiceRepository = invoiceRepository;
        _memberRepository = memberRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DateTime asOfDate, Guid? customerId = null)
    {
        var spec = new UnpaidInvoicesForReportSpec(customerId);
        var invoices = await _invoiceRepository.ListAsync(spec);
        var members = await _memberRepository.ListAsync();
        var memberDict = members.ToDictionary(m => m.Id, m => m.MemberName);

        string? customerName = null;
        if (customerId.HasValue && memberDict.TryGetValue(customerId.Value, out var name))
        {
            customerName = name;
        }

        // Calculate aging buckets
        var agingData = invoices.Select(inv => new AgingItem
        {
            CustomerName = memberDict.TryGetValue(inv.MemberId, out var memberName) ? memberName : "Unknown",
            InvoiceNumber = inv.InvoiceNumber,
            InvoiceDate = inv.InvoiceDate,
            DueDate = inv.DueDate,
            TotalAmount = inv.TotalAmount,
            PaidAmount = inv.PaidAmount,
            OutstandingBalance = inv.TotalAmount - inv.PaidAmount,
            DaysOverdue = (asOfDate - inv.DueDate).Days,
            AgingBucket = GetAgingBucket((asOfDate - inv.DueDate).Days)
        }).Where(a => a.OutstandingBalance > 0).ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, asOfDate, customerName));
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

    private static void ComposeHeader(IContainer container, DateTime asOfDate, string? customerName)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("AGED RECEIVABLES REPORT")
                        .FontSize(20).Bold().FontColor(Colors.Orange.Darken2);
                    col.Item().PaddingTop(5).Text("Accounting Module - Accounts Receivable").FontSize(12).SemiBold();
                    col.Item().Text($"As Of: {asOfDate:MMMM dd, yyyy}").FontSize(10).SemiBold();
                    col.Item().Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    if (!string.IsNullOrEmpty(customerName))
                        col.Item().Text($"Customer: {customerName}").FontSize(9).SemiBold();
                    else
                        col.Item().Text("All Customers").FontSize(9).SemiBold();
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
                    columns.RelativeColumn(2);    // Customer
                    columns.ConstantColumn(80);   // Invoice #
                    columns.ConstantColumn(80);   // Invoice Date
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
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Customer").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Invoice #").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Invoice Date").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Due Date").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Total").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Paid").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Outstanding").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Days").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Orange.Darken2).Padding(5)
                        .Text("Aging").FontColor(Colors.White).FontSize(9).SemiBold();
                });

                // Data rows
                var isAlternate = false;
                foreach (var item in items.OrderBy(i => i.CustomerName).ThenByDescending(i => i.DaysOverdue))
                {
                    var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.CustomerName).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.InvoiceNumber).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(item.InvoiceDate.ToString("MM/dd/yyyy")).FontSize(8);
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
        public string CustomerName { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal OutstandingBalance { get; set; }
        public int DaysOverdue { get; set; }
        public string AgingBucket { get; set; } = string.Empty;
    }
}

/// <summary>
/// Specification for filtering unpaid invoices for aging reports.
/// </summary>
internal sealed class UnpaidInvoicesForReportSpec : Specification<Invoice>
{
    public UnpaidInvoicesForReportSpec(Guid? memberId = null)
    {
        // Filter invoices where paid amount is less than total (outstanding balance)
        Query.Where(inv => inv.PaidAmount < inv.TotalAmount);

        if (memberId.HasValue)
        {
            Query.Where(inv => inv.MemberId == memberId.Value);
        }

        Query.OrderBy(inv => inv.MemberId).ThenBy(inv => inv.DueDate);
    }
}
