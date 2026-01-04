using Accounting.Application.FinancialStatements.Queries.GenerateIncomeStatement.v1;
using Accounting.Application.Reports.IncomeStatement.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Income Statement PDF reports using QuestPDF.
/// </summary>
public sealed class IncomeStatementReportService : IIncomeStatementReportService
{
    private readonly ISender _mediator;

    public IncomeStatementReportService(ISender mediator)
    {
        _mediator = mediator;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(
        DateTime startDate,
        DateTime endDate,
        bool includeComparative = false,
        DateTime? comparativeStartDate = null,
        DateTime? comparativeEndDate = null)
    {
        // Get income statement data using existing query
        var query = new GenerateIncomeStatementQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            IncludeComparativePeriod = includeComparative,
            ComparativeStartDate = comparativeStartDate,
            ComparativeEndDate = comparativeEndDate
        };

        var incomeStatement = await _mediator.Send(query);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, incomeStatement, includeComparative));
                page.Content().Element(c => ComposeContent(c, incomeStatement, includeComparative));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, IncomeStatementDto incomeStatement, bool includeComparative)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Column(col =>
            {
                col.Item().Text("INCOME STATEMENT")
                    .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                col.Item().PaddingTop(5).Text("Accounting Module").FontSize(12).SemiBold();
                col.Item().Text($"Period: {incomeStatement.StartDate:MMM dd, yyyy} - {incomeStatement.EndDate:MMM dd, yyyy}")
                    .FontSize(11).SemiBold();
                
                if (includeComparative && incomeStatement.ComparativePeriod != null)
                {
                    col.Item().Text($"Comparative: {incomeStatement.ComparativePeriod.StartDate:MMM dd, yyyy} - {incomeStatement.ComparativePeriod.EndDate:MMM dd, yyyy}")
                        .FontSize(9).Italic();
                }
                
                col.Item().PaddingTop(3).Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(8);
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, IncomeStatementDto incomeStatement, bool includeComparative)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Revenue Section
            column.Item().Element(c => ComposeSection(c, incomeStatement.Revenue, "REVENUE", includeComparative, false));
            
            column.Item().PaddingTop(10);

            // Cost of Goods Sold Section
            column.Item().Element(c => ComposeSection(c, incomeStatement.CostOfGoodsSold, "COST OF GOODS SOLD", includeComparative, true));
            
            // Gross Profit
            column.Item().PaddingTop(8).Background(Colors.Grey.Lighten3).Padding(8).Row(row =>
            {
                row.RelativeItem().Text("GROSS PROFIT").FontSize(11).Bold();
                
                if (includeComparative)
                {
                    var currentGP = incomeStatement.GrossProfit;
                    var comparativeGP = incomeStatement.ComparativePeriod != null 
                        ? incomeStatement.ComparativePeriod.GrossProfit 
                        : 0;
                    var variance = currentGP - comparativeGP;
                    
                    row.ConstantItem(100).AlignRight().Text(currentGP.ToString("N2")).FontSize(11).Bold();
                    row.ConstantItem(100).AlignRight().Text(comparativeGP.ToString("N2")).FontSize(11).Bold();
                    row.ConstantItem(100).AlignRight().Text(variance.ToString("N2")).FontSize(11).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(incomeStatement.GrossProfit.ToString("N2")).FontSize(11).Bold();
                }
            });
            
            column.Item().PaddingTop(10);

            // Operating Expenses Section
            column.Item().Element(c => ComposeSection(c, incomeStatement.OperatingExpenses, "OPERATING EXPENSES", includeComparative, true));
            
            // Operating Income
            column.Item().PaddingTop(8).Background(Colors.Grey.Lighten3).Padding(8).Row(row =>
            {
                row.RelativeItem().Text("OPERATING INCOME").FontSize(11).Bold();
                
                if (includeComparative)
                {
                    var currentOI = incomeStatement.OperatingIncome;
                    var comparativeOI = incomeStatement.ComparativePeriod != null 
                        ? incomeStatement.ComparativePeriod.OperatingIncome 
                        : 0;
                    var variance = currentOI - comparativeOI;
                    
                    row.ConstantItem(100).AlignRight().Text(currentOI.ToString("N2")).FontSize(11).Bold();
                    row.ConstantItem(100).AlignRight().Text(comparativeOI.ToString("N2")).FontSize(11).Bold();
                    row.ConstantItem(100).AlignRight().Text(variance.ToString("N2")).FontSize(11).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(incomeStatement.OperatingIncome.ToString("N2")).FontSize(11).Bold();
                }
            });
            
            column.Item().PaddingTop(10);

            // Other Income Section
            if (incomeStatement.OtherIncome.Lines.Any())
            {
                column.Item().Element(c => ComposeSection(c, incomeStatement.OtherIncome, "OTHER INCOME", includeComparative, false));
                column.Item().PaddingTop(10);
            }

            // Other Expenses Section
            if (incomeStatement.OtherExpenses.Lines.Any())
            {
                column.Item().Element(c => ComposeSection(c, incomeStatement.OtherExpenses, "OTHER EXPENSES", includeComparative, true));
                column.Item().PaddingTop(10);
            }

            // Net Income
            column.Item().PaddingTop(10).BorderTop(2).BorderColor(Colors.Blue.Darken2)
                .Background(Colors.Blue.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Text("NET INCOME").FontSize(12).Bold();
                
                if (includeComparative)
                {
                    var currentNI = incomeStatement.NetIncome;
                    var comparativeNI = incomeStatement.ComparativePeriod != null 
                        ? incomeStatement.ComparativePeriod.NetIncome 
                        : 0;
                    var variance = currentNI - comparativeNI;
                    
                    row.ConstantItem(100).AlignRight().Text(currentNI.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(comparativeNI.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(variance.ToString("N2")).FontSize(12).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(incomeStatement.NetIncome.ToString("N2")).FontSize(12).Bold();
                }
            });
        });
    }

    private static void ComposeSection(
        IContainer container, 
        IncomeStatementSectionDto section, 
        string sectionTitle, 
        bool includeComparative,
        bool isExpense)
    {
        if (!section.Lines.Any())
            return;

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
                    row.ConstantItem(100).AlignRight().Text("Variance").FontSize(10).SemiBold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text("Amount").FontSize(10).SemiBold();
                }
            });

            // Lines
            foreach (var line in section.Lines)
            {
                column.Item().PaddingLeft(20).PaddingTop(3).Row(row =>
                {
                    row.RelativeItem().Text($"{line.AccountCode} - {line.AccountName}").FontSize(9);
                    
                    if (includeComparative)
                    {
                        row.ConstantItem(100).AlignRight().Text(line.Amount.ToString("N2")).FontSize(9);
                        row.ConstantItem(100).AlignRight().Text(line.ComparativeAmount?.ToString("N2") ?? "-").FontSize(9);
                        row.ConstantItem(100).AlignRight().Text(line.Variance?.ToString("N2") ?? "-").FontSize(9);
                    }
                    else
                    {
                        row.ConstantItem(120).AlignRight().Text(line.Amount.ToString("N2")).FontSize(9);
                    }
                });
            }

            // Section Total
            column.Item().PaddingLeft(20).PaddingTop(6).BorderTop(1).BorderColor(Colors.Grey.Lighten1)
                .Row(row =>
            {
                row.RelativeItem().Text($"Total {sectionTitle}").FontSize(10).SemiBold();
                
                if (includeComparative)
                {
                    var currentTotal = section.Total;
                    var priorTotal = section.Lines.Sum(l => l.ComparativeAmount ?? 0);
                    var variance = currentTotal - priorTotal;
                    
                    row.ConstantItem(100).AlignRight().Text(currentTotal.ToString("N2")).FontSize(10).SemiBold();
                    row.ConstantItem(100).AlignRight().Text(priorTotal.ToString("N2")).FontSize(10).SemiBold();
                    row.ConstantItem(100).AlignRight().Text(variance.ToString("N2")).FontSize(10).SemiBold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(section.Total.ToString("N2")).FontSize(10).SemiBold();
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
