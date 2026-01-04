using Accounting.Application.Reports.JournalEntry.v1.Services;
using Ardalis.Specification;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Journal Entry PDF reports using QuestPDF.
/// </summary>
public sealed class JournalEntryReportService : IJournalEntryReportService
{
    private readonly IReadRepository<JournalEntry> _journalEntryRepository;

    public JournalEntryReportService(IReadRepository<JournalEntry> journalEntryRepository)
    {
        _journalEntryRepository = journalEntryRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DateTime startDate, DateTime endDate, bool isPostedOnly = false)
    {
        var spec = new JournalEntryForReportSpec(startDate, endDate, isPostedOnly);
        var entries = await _journalEntryRepository.ListAsync(spec);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, startDate, endDate, isPostedOnly));
                page.Content().Element(c => ComposeContent(c, entries.ToList()));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, DateTime startDate, DateTime endDate, bool isPostedOnly)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("JOURNAL ENTRY REPORT")
                        .FontSize(20).Bold().FontColor(Colors.Green.Darken2);
                    col.Item().PaddingTop(5).Text("Accounting Module").FontSize(12).SemiBold();
                    col.Item().Text($"Period: {startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}").FontSize(10).SemiBold();
                    col.Item().Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().Text(isPostedOnly ? "Posted Entries Only" : "All Entries").FontSize(9).SemiBold();
                });
            });

            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium);
        });
    }

    private static void ComposeContent(IContainer container, List<JournalEntry> entries)
    {
        container.PaddingTop(15).Column(column =>
        {
            // Summary section
            column.Item().Row(row =>
            {
                row.RelativeItem().Background(Colors.Grey.Lighten4).Padding(10).Column(col =>
                {
                    col.Item().Text("Summary").FontSize(12).SemiBold();
                    col.Item().Text($"Total Entries: {entries.Count}").FontSize(10);
                    col.Item().Text($"Posted: {entries.Count(e => e.IsPosted)}").FontSize(10);
                    col.Item().Text($"Pending: {entries.Count(e => !e.IsPosted)}").FontSize(10);
                    col.Item().Text($"Total Original Amount: {entries.Sum(e => e.OriginalAmount):N2}").FontSize(10);
                });
            });

            column.Item().PaddingTop(15);

            // Table
            column.Item().Table(table =>
            {
                // Column definitions
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(80);   // Date
                    columns.ConstantColumn(100);  // Reference
                    columns.RelativeColumn(2);    // Description
                    columns.ConstantColumn(80);   // Source
                    columns.ConstantColumn(80);   // Status
                    columns.ConstantColumn(60);   // Posted
                    columns.ConstantColumn(80);   // Amount
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Date").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Reference").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Description").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Source").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Status").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Posted").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Green.Darken2).Padding(5)
                        .Text("Amount").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                });

                // Data rows
                var isAlternate = false;
                foreach (var entry in entries.OrderBy(e => e.Date).ThenBy(e => e.ReferenceNumber))
                {
                    var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.Date.ToString("MM/dd/yyyy")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.ReferenceNumber).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.Description ?? "-").FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.Source).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.Status).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.IsPosted ? "Yes" : "No").FontSize(8)
                        .FontColor(entry.IsPosted ? Colors.Green.Darken1 : Colors.Orange.Darken1);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(entry.OriginalAmount.ToString("N2")).FontSize(8);

                    isAlternate = !isAlternate;
                }

                // Totals row
                table.Cell().ColumnSpan(6).Background(Colors.Grey.Lighten2).Padding(5)
                    .Text("TOTAL").FontSize(9).SemiBold().AlignRight();
                table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                    .Text(entries.Sum(e => e.OriginalAmount).ToString("N2")).FontSize(9).SemiBold();
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
}

/// <summary>
/// Specification for filtering Journal Entries for reports.
/// </summary>
internal sealed class JournalEntryForReportSpec : Specification<JournalEntry>
{
    public JournalEntryForReportSpec(DateTime startDate, DateTime endDate, bool isPostedOnly = false)
    {
        Query.Where(je => je.Date >= startDate && je.Date <= endDate);

        if (isPostedOnly)
        {
            Query.Where(je => je.IsPosted);
        }

        Query.OrderBy(je => je.Date).ThenBy(je => je.ReferenceNumber);
    }
}
