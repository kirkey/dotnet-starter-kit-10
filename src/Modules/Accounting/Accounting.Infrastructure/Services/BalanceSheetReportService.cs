using Accounting.Application.FinancialStatements.Queries.GenerateBalanceSheet.v1;
using Accounting.Application.Reports.BalanceSheet.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Balance Sheet PDF reports using QuestPDF.
/// </summary>
public sealed class BalanceSheetReportService : IBalanceSheetReportService
{
    private readonly ISender _mediator;

    public BalanceSheetReportService(ISender mediator)
    {
        _mediator = mediator;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(
        DateTime asOfDate, 
        bool includeComparative = false, 
        DateTime? comparativeAsOfDate = null)
    {
        // Get balance sheet data using existing query
        var query = new GenerateBalanceSheetQuery
        {
            AsOfDate = asOfDate,
            IncludeComparativePeriod = includeComparative,
            ComparativeAsOfDate = comparativeAsOfDate
        };

        var balanceSheet = await _mediator.Send(query);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, balanceSheet));
                page.Content().Element(c => ComposeContent(c, balanceSheet, includeComparative));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, BalanceSheetDto balanceSheet)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Column(col =>
            {
                col.Item().Text("BALANCE SHEET")
                    .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                col.Item().PaddingTop(5).Text("Accounting Module").FontSize(12).SemiBold();
                col.Item().Text($"As of {balanceSheet.AsOfDate:MMMM dd, yyyy}").FontSize(11).SemiBold();
                if (balanceSheet.ComparativePeriod != null)
                {
                    col.Item().Text($"With Comparative Period: {balanceSheet.ComparativePeriod.AsOfDate:MMMM dd, yyyy}")
                        .FontSize(9).Italic();
                }
                col.Item().PaddingTop(3).Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(8);
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, BalanceSheetDto balanceSheet, bool includeComparative)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Assets Section
            column.Item().Element(c => ComposeSection(c, balanceSheet.Assets, "ASSETS", includeComparative));
            
            column.Item().PaddingTop(10);

            // Liabilities Section
            column.Item().Element(c => ComposeSection(c, balanceSheet.Liabilities, "LIABILITIES", includeComparative));
            
            column.Item().PaddingTop(10);

            // Equity Section
            column.Item().Element(c => ComposeSection(c, balanceSheet.Equity, "EQUITY", includeComparative));
            
            column.Item().PaddingTop(20);

            // Summary Totals
            column.Item().Element(c => ComposeSummary(c, balanceSheet, includeComparative));
        });
    }

    private static void ComposeSection(
        IContainer container, 
        BalanceSheetSectionDto section, 
        string sectionTitle, 
        bool includeComparative)
    {
        container.Column(column =>
        {
            // Section Header
            column.Item().Background(Colors.Grey.Lighten2).Padding(8).Row(row =>
            {
                row.RelativeItem().Text(sectionTitle).FontSize(12).Bold();
                if (includeComparative)
                {
                    row.ConstantItem(100).AlignRight().Text("Current").FontSize(10).SemiBold();
                    row.ConstantItem(100).AlignRight().Text("Prior").FontSize(10).SemiBold();
                    row.ConstantItem(100).AlignRight().Text("Change").FontSize(10).SemiBold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text("Amount").FontSize(10).SemiBold();
                }
            });

            // Sub-sections
            foreach (var subSection in section.SubSections)
            {
                // Sub-section header
                column.Item().PaddingTop(8).PaddingLeft(10).Row(row =>
                {
                    row.RelativeItem().Text(subSection.SubSectionName).FontSize(11).SemiBold();
                });

                // Lines
                foreach (var line in subSection.Lines)
                {
                    column.Item().PaddingLeft(20).PaddingTop(2).Row(row =>
                    {
                        row.RelativeItem().Text($"{line.AccountCode} - {line.AccountName}").FontSize(9);
                        
                        if (includeComparative)
                        {
                            row.ConstantItem(100).AlignRight().Text(line.Amount.ToString("N2")).FontSize(9);
                            row.ConstantItem(100).AlignRight().Text(line.ComparativeAmount?.ToString("N2") ?? "-").FontSize(9);
                            row.ConstantItem(100).AlignRight().Text(line.Change?.ToString("N2") ?? "-").FontSize(9);
                        }
                        else
                        {
                            row.ConstantItem(120).AlignRight().Text(line.Amount.ToString("N2")).FontSize(9);
                        }
                    });
                }

                // Sub-section total
                column.Item().PaddingLeft(20).PaddingTop(4).BorderTop(1).BorderColor(Colors.Grey.Lighten1)
                    .Row(row =>
                    {
                        row.RelativeItem().Text($"Total {subSection.SubSectionName}").FontSize(10).SemiBold();
                        
                        if (includeComparative)
                        {
                            var currentTotal = subSection.Lines.Sum(l => l.Amount);
                            var priorTotal = subSection.Lines.Sum(l => l.ComparativeAmount ?? 0);
                            var changeTotal = currentTotal - priorTotal;
                            
                            row.ConstantItem(100).AlignRight().Text(currentTotal.ToString("N2")).FontSize(10).SemiBold();
                            row.ConstantItem(100).AlignRight().Text(priorTotal.ToString("N2")).FontSize(10).SemiBold();
                            row.ConstantItem(100).AlignRight().Text(changeTotal.ToString("N2")).FontSize(10).SemiBold();
                        }
                        else
                        {
                            row.ConstantItem(120).AlignRight().Text(subSection.Total.ToString("N2")).FontSize(10).SemiBold();
                        }
                    });
            }

            // Section Total
            column.Item().PaddingTop(8).Background(Colors.Grey.Lighten3).Padding(8)
                .Row(row =>
                {
                    row.RelativeItem().Text($"TOTAL {sectionTitle}").FontSize(11).Bold();
                    
                    if (includeComparative)
                    {
                        // Calculate comparative totals
                        var currentTotal = section.Total;
                        var priorTotal = section.SubSections.Sum(ss => ss.Lines.Sum(l => l.ComparativeAmount ?? 0));
                        var changeTotal = currentTotal - priorTotal;
                        
                        row.ConstantItem(100).AlignRight().Text(currentTotal.ToString("N2")).FontSize(11).Bold();
                        row.ConstantItem(100).AlignRight().Text(priorTotal.ToString("N2")).FontSize(11).Bold();
                        row.ConstantItem(100).AlignRight().Text(changeTotal.ToString("N2")).FontSize(11).Bold();
                    }
                    else
                    {
                        row.ConstantItem(120).AlignRight().Text(section.Total.ToString("N2")).FontSize(11).Bold();
                    }
                });
        });
    }

    private static void ComposeSummary(IContainer container, BalanceSheetDto balanceSheet, bool includeComparative)
    {
        container.Column(column =>
        {
            column.Item().BorderTop(2).BorderColor(Colors.Blue.Darken2).PaddingTop(10);

            // Total Assets
            column.Item().Background(Colors.Blue.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Text("TOTAL ASSETS").FontSize(12).Bold();
                
                if (includeComparative)
                {
                    var currentAssets = balanceSheet.TotalAssets;
                    var priorAssets = balanceSheet.Assets.SubSections.Sum(ss => ss.Lines.Sum(l => l.ComparativeAmount ?? 0));
                    var changeAssets = currentAssets - priorAssets;
                    
                    row.ConstantItem(100).AlignRight().Text(currentAssets.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(priorAssets.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(changeAssets.ToString("N2")).FontSize(12).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(balanceSheet.TotalAssets.ToString("N2")).FontSize(12).Bold();
                }
            });

            column.Item().PaddingTop(5);

            // Total Liabilities and Equity
            column.Item().Background(Colors.Blue.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Text("TOTAL LIABILITIES & EQUITY").FontSize(12).Bold();
                
                if (includeComparative)
                {
                    var currentLiabEquity = balanceSheet.TotalLiabilitiesAndEquity;
                    var priorLiabilities = balanceSheet.Liabilities.SubSections.Sum(ss => ss.Lines.Sum(l => l.ComparativeAmount ?? 0));
                    var priorEquity = balanceSheet.Equity.SubSections.Sum(ss => ss.Lines.Sum(l => l.ComparativeAmount ?? 0));
                    var priorLiabEquity = priorLiabilities + priorEquity;
                    var changeLiabEquity = currentLiabEquity - priorLiabEquity;
                    
                    row.ConstantItem(100).AlignRight().Text(currentLiabEquity.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(priorLiabEquity.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(changeLiabEquity.ToString("N2")).FontSize(12).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(balanceSheet.TotalLiabilitiesAndEquity.ToString("N2")).FontSize(12).Bold();
                }
            });

            // Balance Check
            column.Item().PaddingTop(10).Row(row =>
            {
                if (balanceSheet.IsBalanced)
                {
                    row.RelativeItem().AlignCenter().Text("✓ Balance Sheet is in balance")
                        .FontSize(10).SemiBold().FontColor(Colors.Green.Darken2);
                }
                else
                {
                    var difference = balanceSheet.TotalAssets - balanceSheet.TotalLiabilitiesAndEquity;
                    row.RelativeItem().AlignCenter().Text($"⚠ Out of Balance: {difference:N2}")
                        .FontSize(10).SemiBold().FontColor(Colors.Red.Darken2);
                }
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
