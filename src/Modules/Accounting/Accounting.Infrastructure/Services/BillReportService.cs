using Accounting.Application.Reports.Bill.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating individual Bill PDF reports using QuestPDF.
/// </summary>
public sealed class BillReportService : IBillReportService
{
    private readonly IReadRepository<Bill> _billRepository;

    public BillReportService(
        [FromKeyedServices("accounting:bills")] IReadRepository<Bill> billRepository)
    {
        _billRepository = billRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType billId)
    {
        var bill = await _billRepository.GetByIdAsync(billId);
        if (bill == null)
        {
            throw new InvalidOperationException($"Bill with ID {billId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, bill));
                page.Content().Element(c => ComposeContent(c, bill));
                page.Footer().Element(c => ComposeFooter(c, bill));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, Bill bill)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("VENDOR BILL").FontSize(20).Bold().FontColor(Colors.Red.Darken2);
                    col.Item().PaddingTop(5).Text($"# {bill.BillNumber}").FontSize(12).SemiBold();
                });

                row.ConstantItem(250).AlignRight().Column(col =>
                {
                    col.Item().Text("YOUR COMPANY NAME").FontSize(14).Bold();
                    col.Item().PaddingTop(3).Text("123 Business Street").FontSize(8);
                    col.Item().Text("City, State 12345").FontSize(8);
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Red.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, Bill bill)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Bill Details
            column.Item().Row(row =>
            {
                // Vendor Info
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("VENDOR:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Vendor ID: {bill.VendorId}").FontSize(9);
                    if (!string.IsNullOrEmpty(bill.PaymentTerms))
                    {
                        col.Item().Text($"Terms: {bill.PaymentTerms}").FontSize(9);
                    }
                });

                // Bill Info
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Bill Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(bill.BillDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Due Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(bill.DueDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Status:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(bill.Status).FontSize(9).SemiBold()
                            .FontColor(bill.Status == "Paid" ? Colors.Green.Darken2 : Colors.Red.Darken2);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Line Items
            if (bill.LineItems.Any())
            {
                column.Item().Text("LINE ITEMS").FontSize(11).SemiBold();
                column.Item().PaddingTop(8).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(80);
                        columns.ConstantColumn(100);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Description").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Quantity").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Unit Price").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Total").FontSize(9).SemiBold();
                    });

                    var isAlternate = false;
                    foreach (var lineItem in bill.LineItems)
                    {
                        var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                        table.Cell().Background(bgColor).Padding(5)
                            .Text(lineItem.Description ?? "N/A").FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text(lineItem.Quantity.ToString("N2")).FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text(lineItem.UnitPrice.ToString("N2")).FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text((lineItem.Quantity * lineItem.UnitPrice).ToString("N2")).FontSize(9);

                        isAlternate = !isAlternate;
                    }
                });

                column.Item().PaddingTop(15);
            }

            // Totals
            column.Item().Row(row =>
            {
                row.RelativeItem();

                row.ConstantItem(300).Column(col =>
                {
                    col.Item().PaddingTop(5).Background(Colors.Red.Lighten4).Padding(8).Row(r =>
                    {
                        r.RelativeItem().Text("TOTAL AMOUNT:").FontSize(11).Bold();
                        r.ConstantItem(100).AlignRight().Text(bill.TotalAmount.ToString("N2")).FontSize(11).Bold();
                    });
                });
            });

            // Description
            if (!string.IsNullOrEmpty(bill.Description))
            {
                column.Item().PaddingTop(20).Column(col =>
                {
                    col.Item().Text("DESCRIPTION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(bill.Description).FontSize(9);
                });
            }

            // Notes
            if (!string.IsNullOrEmpty(bill.Notes))
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("NOTES:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(bill.Notes).FontSize(9);
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, Bill bill)
    {
        container.Column(column =>
        {
            column.Item().BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10);

            column.Item().Row(row =>
            {
                row.RelativeItem().Text($"Bill #{bill.BillNumber}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                row.RelativeItem().AlignRight().Text($"Generated on {DateTime.Now:MMM dd, yyyy}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }
}
