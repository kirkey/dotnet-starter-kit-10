using Accounting.Application.Reports.TrialBalance.v1.Services;
using Ardalis.Specification;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Trial Balance PDF reports using QuestPDF.
/// </summary>
public sealed class TrialBalanceReportService : ITrialBalanceReportService
{
    private readonly IReadRepository<ChartOfAccount> _accountRepository;
    private readonly IReadRepository<GeneralLedger> _glRepository;

    public TrialBalanceReportService(
        IReadRepository<ChartOfAccount> accountRepository,
        IReadRepository<GeneralLedger> glRepository)
    {
        _accountRepository = accountRepository;
        _glRepository = glRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DateTime asOfDate, Guid? periodId = null)
    {
        // Get all accounts
        var accounts = await _accountRepository.ListAsync();

        // Get all GL entries up to the as-of date
        var spec = new GeneralLedgerUpToDateSpec(asOfDate);
        var glEntries = await _glRepository.ListAsync(spec);

        // Calculate balances per account
        var trialBalanceItems = accounts
            .Where(a => a.IsActive)
            .Select(account =>
            {
                var accountEntries = glEntries.Where(gl => gl.AccountId == account.Id).ToList();
                var totalDebits = accountEntries.Sum(e => e.Debit);
                var totalCredits = accountEntries.Sum(e => e.Credit);
                var balance = totalDebits - totalCredits;

                return new TrialBalanceItem
                {
                    AccountCode = account.AccountCode,
                    AccountName = account.Name,
                    AccountType = account.AccountType,
                    DebitBalance = balance > 0 ? balance : 0,
                    CreditBalance = balance < 0 ? Math.Abs(balance) : 0
                };
            })
            .Where(tb => tb.DebitBalance != 0 || tb.CreditBalance != 0)
            .OrderBy(tb => tb.AccountCode)
            .ToList();

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, asOfDate));
                page.Content().Element(c => ComposeContent(c, trialBalanceItems));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, DateTime asOfDate)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Column(col =>
            {
                col.Item().AlignCenter().Text("TRIAL BALANCE")
                    .FontSize(20).Bold().FontColor(Colors.Purple.Darken2);
                col.Item().AlignCenter().PaddingTop(5).Text("Accounting Module").FontSize(12).SemiBold();
                col.Item().AlignCenter().Text($"As Of: {asOfDate:MMMM dd, yyyy}").FontSize(10).SemiBold();
                col.Item().AlignCenter().Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(9);
            });

            column.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.Grey.Medium);
        });
    }

    private static void ComposeContent(IContainer container, List<TrialBalanceItem> items)
    {
        container.PaddingTop(15).Column(column =>
        {
            // Table
            column.Item().Table(table =>
            {
                // Column definitions
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(80);   // Account Code
                    columns.RelativeColumn(2);    // Account Name
                    columns.ConstantColumn(100);  // Account Type
                    columns.ConstantColumn(100);  // Debit
                    columns.ConstantColumn(100);  // Credit
                });

                // Header
                table.Header(header =>
                {
                    header.Cell().Background(Colors.Purple.Darken2).Padding(5)
                        .Text("Account Code").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Purple.Darken2).Padding(5)
                        .Text("Account Name").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Purple.Darken2).Padding(5)
                        .Text("Type").FontColor(Colors.White).FontSize(9).SemiBold();
                    header.Cell().Background(Colors.Purple.Darken2).Padding(5)
                        .Text("Debit").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                    header.Cell().Background(Colors.Purple.Darken2).Padding(5)
                        .Text("Credit").FontColor(Colors.White).FontSize(9).SemiBold().AlignRight();
                });

                // Group by account type
                var groupedItems = items.GroupBy(i => i.AccountType).OrderBy(g => GetAccountTypeOrder(g.Key));

                foreach (var group in groupedItems)
                {
                    // Group header
                    table.Cell().ColumnSpan(5).Background(Colors.Grey.Lighten3).Padding(5)
                        .Text(group.Key).FontSize(10).SemiBold();

                    var isAlternate = false;
                    foreach (var item in group.OrderBy(i => i.AccountCode))
                    {
                        var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                        table.Cell().Background(bgColor).Padding(4)
                            .Text(item.AccountCode).FontSize(9);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(item.AccountName).FontSize(9);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(item.AccountType).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4).AlignRight()
                            .Text(item.DebitBalance > 0 ? item.DebitBalance.ToString("N2") : "-").FontSize(9);
                        table.Cell().Background(bgColor).Padding(4).AlignRight()
                            .Text(item.CreditBalance > 0 ? item.CreditBalance.ToString("N2") : "-").FontSize(9);

                        isAlternate = !isAlternate;
                    }

                    // Group subtotal
                    table.Cell().ColumnSpan(3).Background(Colors.Grey.Lighten2).Padding(4)
                        .Text($"Subtotal - {group.Key}").FontSize(9).SemiBold().AlignRight();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight()
                        .Text(group.Sum(i => i.DebitBalance).ToString("N2")).FontSize(9).SemiBold();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(4).AlignRight()
                        .Text(group.Sum(i => i.CreditBalance).ToString("N2")).FontSize(9).SemiBold();
                }

                // Grand totals row
                var totalDebits = items.Sum(i => i.DebitBalance);
                var totalCredits = items.Sum(i => i.CreditBalance);
                var isBalanced = Math.Abs(totalDebits - totalCredits) < 0.01m;

                table.Cell().ColumnSpan(3).Background(Colors.Purple.Lighten4).Padding(5)
                    .Text("TOTAL").FontSize(10).SemiBold().AlignRight();
                table.Cell().Background(Colors.Purple.Lighten4).Padding(5).AlignRight()
                    .Text(totalDebits.ToString("N2")).FontSize(10).SemiBold();
                table.Cell().Background(Colors.Purple.Lighten4).Padding(5).AlignRight()
                    .Text(totalCredits.ToString("N2")).FontSize(10).SemiBold();

                // Balance indicator
                table.Cell().ColumnSpan(5).Padding(10).AlignCenter()
                    .Text(isBalanced ? "✓ Trial Balance is BALANCED" : "⚠ Trial Balance is OUT OF BALANCE")
                    .FontSize(12).SemiBold()
                    .FontColor(isBalanced ? Colors.Green.Darken2 : Colors.Red.Darken2);
            });
        });
    }

    private static int GetAccountTypeOrder(string accountType)
    {
        return accountType.ToLower() switch
        {
            "asset" => 1,
            "liability" => 2,
            "equity" => 3,
            "revenue" => 4,
            "expense" => 5,
            _ => 99
        };
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

    private class TrialBalanceItem
    {
        public string AccountCode { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal DebitBalance { get; set; }
        public decimal CreditBalance { get; set; }
    }
}

/// <summary>
/// Specification for filtering General Ledger entries up to a specific date.
/// </summary>
internal sealed class GeneralLedgerUpToDateSpec : Specification<GeneralLedger>
{
    public GeneralLedgerUpToDateSpec(DateTime asOfDate)
    {
        Query.Where(gl => gl.TransactionDate <= asOfDate);
    }
}
