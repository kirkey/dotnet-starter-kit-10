using Accounting.Application.Reports.GeneralLedger.v1.Services;
using Ardalis.Specification;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating General Ledger PDF reports using QuestPDF.
/// </summary>
public sealed class GeneralLedgerReportService : IGeneralLedgerReportService
{
    private readonly IReadRepository<GeneralLedger> _glRepository;
    private readonly IReadRepository<ChartOfAccount> _accountRepository;

    public GeneralLedgerReportService(
        IReadRepository<GeneralLedger> glRepository,
        IReadRepository<ChartOfAccount> accountRepository)
    {
        _glRepository = glRepository;
        _accountRepository = accountRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DateTime startDate, DateTime endDate, Guid? accountId = null)
    {
        var spec = new GeneralLedgerForReportSpec(startDate, endDate, accountId);
        var entries = await _glRepository.ListAsync(spec);

        string? accountName = null;
        if (accountId.HasValue)
        {
            var account = await _accountRepository.GetByIdAsync(accountId.Value);
            accountName = account?.Name;
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, startDate, endDate, accountName));
                page.Content().Element(c => ComposeContent(c, entries.ToList()));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, DateTime startDate, DateTime endDate, string? accountName)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("GENERAL LEDGER REPORT")
                        .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(5).Text("Accounting Module").FontSize(12).SemiBold();
                    col.Item().Text($"Period: {startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}").FontSize(10).SemiBold();
                    col.Item().Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    if (!string.IsNullOrEmpty(accountName))
                        col.Item().Text($"Account: {accountName}").FontSize(9).SemiBold();
                    else
                        col.Item().Text("All Accounts").FontSize(9).SemiBold();
                });
            });

            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium);
        });
    }

    private static void ComposeContent(IContainer container, List<GeneralLedger> entries)
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
                    col.Item().Text($"Total Debits: {entries.Sum(e => e.Debit):N2}").FontSize(10);
                    col.Item().Text($"Total Credits: {entries.Sum(e => e.Credit):N2}").FontSize(10);
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
                    columns.ConstantColumn(80);   // Account Code
                    columns.RelativeColumn(2);    // Memo
                    columns.ConstantColumn(100);  // Reference
                    columns.ConstantColumn(80);   // Source
                    columns.ConstantColumn(80);   // Debit
                    columns.ConstantColumn(80);   // Credit
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Date").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Account Code").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Memo").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Reference").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Source").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Debit").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Blue.Darken2).Padding(5)
                        .Text("Credit").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                });

                // Data rows
                var isAlternate = false;
                foreach (var entry in entries.OrderBy(e => e.TransactionDate).ThenBy(e => e.AccountCode))
                {
                    var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.TransactionDate.ToString("MM/dd/yyyy")).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.AccountCode).FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.Memo ?? "-").FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.ReferenceNumber ?? "-").FontSize(8);
                    table.Cell().Background(bgColor).Padding(4)
                        .Text(entry.Source ?? "-").FontSize(8);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(entry.Debit > 0 ? entry.Debit.ToString("N2") : "-").FontSize(8);
                    table.Cell().Background(bgColor).Padding(4).AlignRight()
                        .Text(entry.Credit > 0 ? entry.Credit.ToString("N2") : "-").FontSize(8);

                    isAlternate = !isAlternate;
                }

                // Totals row
                table.Cell().ColumnSpan(5).Background(Colors.Grey.Lighten2).Padding(5)
                    .Text("TOTAL").FontSize(9).SemiBold().AlignRight();
                table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                    .Text(entries.Sum(e => e.Debit).ToString("N2")).FontSize(9).SemiBold();
                table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                    .Text(entries.Sum(e => e.Credit).ToString("N2")).FontSize(9).SemiBold();
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
/// Specification for filtering General Ledger entries for reports.
/// </summary>
internal sealed class GeneralLedgerForReportSpec : Specification<GeneralLedger>
{
    public GeneralLedgerForReportSpec(DateTime startDate, DateTime endDate, Guid? accountId = null)
    {
        Query.Where(gl => gl.TransactionDate >= startDate && gl.TransactionDate <= endDate);

        if (accountId.HasValue)
        {
            Query.Where(gl => gl.AccountId == accountId.Value);
        }

        Query.OrderBy(gl => gl.TransactionDate).ThenBy(gl => gl.AccountCode);
    }
}
