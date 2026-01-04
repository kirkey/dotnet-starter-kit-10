using Accounting.Application.FinancialStatements.Queries.GenerateCashFlowStatement.v1;
using Accounting.Application.Reports.CashFlowStatement.v1.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Cash Flow Statement PDF reports using QuestPDF.
/// </summary>
public sealed class CashFlowStatementReportService : ICashFlowStatementReportService
{
    private readonly ISender _mediator;

    public CashFlowStatementReportService(ISender mediator)
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
        // Get cash flow statement data using existing query
        var query = new GenerateCashFlowStatementQuery
        {
            StartDate = startDate,
            EndDate = endDate,
            Method = "Direct",
            IncludeComparativePeriod = includeComparative,
            ComparativeStartDate = comparativeStartDate,
            ComparativeEndDate = comparativeEndDate
        };

        var cashFlowStatement = await _mediator.Send(query);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, cashFlowStatement, includeComparative));
                page.Content().Element(c => ComposeContent(c, cashFlowStatement, includeComparative));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, CashFlowStatementDto cashFlowStatement, bool includeComparative)
    {
        container.Column(column =>
        {
            column.Item().AlignCenter().Column(col =>
            {
                col.Item().Text("CASH FLOW STATEMENT")
                    .FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                col.Item().PaddingTop(5).Text("Accounting Module").FontSize(12).SemiBold();
                col.Item().Text($"Period: {cashFlowStatement.StartDate:MMM dd, yyyy} - {cashFlowStatement.EndDate:MMM dd, yyyy}")
                    .FontSize(11).SemiBold();
                col.Item().Text($"Method: {cashFlowStatement.Method}").FontSize(10).Italic();
                
                if (includeComparative && cashFlowStatement.ComparativePeriod != null)
                {
                    col.Item().Text($"Comparative: {cashFlowStatement.ComparativePeriod.StartDate:MMM dd, yyyy} - {cashFlowStatement.ComparativePeriod.EndDate:MMM dd, yyyy}")
                        .FontSize(9).Italic();
                }
                
                col.Item().PaddingTop(3).Text($"Report Generated: {DateTime.Now:MMMM dd, yyyy hh:mm tt}").FontSize(8);
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, CashFlowStatementDto cashFlowStatement, bool includeComparative)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Operating Activities Section
            column.Item().Element(c => ComposeSection(c, cashFlowStatement.OperatingActivities, "CASH FLOWS FROM OPERATING ACTIVITIES", includeComparative));
            
            column.Item().PaddingTop(10);

            // Investing Activities Section
            column.Item().Element(c => ComposeSection(c, cashFlowStatement.InvestingActivities, "CASH FLOWS FROM INVESTING ACTIVITIES", includeComparative));
            
            column.Item().PaddingTop(10);

            // Financing Activities Section
            column.Item().Element(c => ComposeSection(c, cashFlowStatement.FinancingActivities, "CASH FLOWS FROM FINANCING ACTIVITIES", includeComparative));
            
            column.Item().PaddingTop(15);

            // Net Cash Flow
            column.Item().Background(Colors.Grey.Lighten3).Padding(8).Row(row =>
            {
                row.RelativeItem().Text("NET INCREASE (DECREASE) IN CASH").FontSize(11).Bold();
                
                if (includeComparative)
                {
                    var currentNet = cashFlowStatement.NetCashFlow;
                    var comparativeNet = cashFlowStatement.ComparativePeriod != null 
                        ? cashFlowStatement.ComparativePeriod.NetCashFlow 
                        : 0;
                    var change = currentNet - comparativeNet;
                    
                    row.ConstantItem(100).AlignRight().Text(currentNet.ToString("N2")).FontSize(11).Bold();
                    row.ConstantItem(100).AlignRight().Text(comparativeNet.ToString("N2")).FontSize(11).Bold();
                    row.ConstantItem(100).AlignRight().Text(change.ToString("N2")).FontSize(11).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(cashFlowStatement.NetCashFlow.ToString("N2")).FontSize(11).Bold();
                }
            });

            column.Item().PaddingTop(10);

            // Beginning Cash Balance
            column.Item().PaddingLeft(20).Row(row =>
            {
                row.RelativeItem().Text("Cash at Beginning of Period").FontSize(10);
                
                if (includeComparative)
                {
                    var currentBeginning = cashFlowStatement.BeginningCashBalance;
                    var comparativeBeginning = cashFlowStatement.ComparativePeriod != null 
                        ? cashFlowStatement.ComparativePeriod.BeginningCashBalance 
                        : 0;
                    var change = currentBeginning - comparativeBeginning;
                    
                    row.ConstantItem(100).AlignRight().Text(currentBeginning.ToString("N2")).FontSize(10);
                    row.ConstantItem(100).AlignRight().Text(comparativeBeginning.ToString("N2")).FontSize(10);
                    row.ConstantItem(100).AlignRight().Text(change.ToString("N2")).FontSize(10);
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(cashFlowStatement.BeginningCashBalance.ToString("N2")).FontSize(10);
                }
            });

            // Ending Cash Balance
            column.Item().PaddingTop(5).BorderTop(2).BorderColor(Colors.Blue.Darken2)
                .Background(Colors.Blue.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Text("CASH AT END OF PERIOD").FontSize(12).Bold();
                
                if (includeComparative)
                {
                    var currentEnding = cashFlowStatement.EndingCashBalance;
                    var comparativeEnding = cashFlowStatement.ComparativePeriod != null 
                        ? cashFlowStatement.ComparativePeriod.EndingCashBalance 
                        : 0;
                    var change = currentEnding - comparativeEnding;
                    
                    row.ConstantItem(100).AlignRight().Text(currentEnding.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(comparativeEnding.ToString("N2")).FontSize(12).Bold();
                    row.ConstantItem(100).AlignRight().Text(change.ToString("N2")).FontSize(12).Bold();
                }
                else
                {
                    row.ConstantItem(120).AlignRight().Text(cashFlowStatement.EndingCashBalance.ToString("N2")).FontSize(12).Bold();
                }
            });
        });
    }

    private static void ComposeSection(
        IContainer container, 
        CashFlowSectionDto section, 
        string sectionTitle, 
        bool includeComparative)
    {
        if (!section.Lines.Any())
            return;

        container.Column(column =>
        {
            // Section Header
            column.Item().Background(Colors.Grey.Lighten2).Padding(8).Row(row =>
            {
                row.RelativeItem().Text(sectionTitle).FontSize(11).Bold();
                
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

            // Lines
            foreach (var line in section.Lines)
            {
                column.Item().PaddingLeft(20).PaddingTop(3).Row(row =>
                {
                    row.RelativeItem().Text(line.Description).FontSize(9);
                    
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

            // Section Total
            column.Item().PaddingLeft(20).PaddingTop(6).BorderTop(1).BorderColor(Colors.Grey.Lighten1)
                .Row(row =>
            {
                var shortTitle = sectionTitle.Replace("CASH FLOWS FROM ", "");
                row.RelativeItem().Text($"Net Cash from {shortTitle}").FontSize(10).SemiBold();
                
                if (includeComparative)
                {
                    var currentTotal = section.Total;
                    var priorTotal = section.Lines.Sum(l => l.ComparativeAmount ?? 0);
                    var change = currentTotal - priorTotal;
                    
                    row.ConstantItem(100).AlignRight().Text(currentTotal.ToString("N2")).FontSize(10).SemiBold();
                    row.ConstantItem(100).AlignRight().Text(priorTotal.ToString("N2")).FontSize(10).SemiBold();
                    row.ConstantItem(100).AlignRight().Text(change.ToString("N2")).FontSize(10).SemiBold();
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
