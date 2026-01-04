using Accounting.Application.Reports.VendorStatement.v1.Services;
using Ardalis.Specification;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Vendor Statement PDF reports using QuestPDF.
/// </summary>
public sealed class VendorStatementReportService : IVendorStatementReportService
{
    private readonly IReadRepository<Vendor> _vendorRepository;
    private readonly IReadRepository<Bill> _billRepository;

    public VendorStatementReportService(
        [FromKeyedServices("accounting:vendors")] IReadRepository<Vendor> vendorRepository,
        [FromKeyedServices("accounting:bills")] IReadRepository<Bill> billRepository)
    {
        _vendorRepository = vendorRepository;
        _billRepository = billRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType vendorId, DateTime startDate, DateTime endDate)
    {
        var vendor = await _vendorRepository.GetByIdAsync(vendorId);
        if (vendor == null)
        {
            throw new InvalidOperationException($"Vendor with ID {vendorId} not found.");
        }

        // Get bills for the period
        var spec = new BillsForStatementSpec(vendorId, startDate, endDate);
        var bills = await _billRepository.ListAsync(spec);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, vendor, startDate, endDate));
                page.Content().Element(c => ComposeContent(c, vendor, bills.ToList(), startDate, endDate));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, Vendor vendor, DateTime startDate, DateTime endDate)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("YOUR COMPANY NAME").FontSize(18).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(3).Text("123 Business Street").FontSize(9);
                    col.Item().Text("City, State 12345").FontSize(9);
                    col.Item().Text("Phone: (555) 123-4567").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().Text("VENDOR STATEMENT").FontSize(16).Bold().FontColor(Colors.Red.Darken2);
                    col.Item().PaddingTop(5).Text($"Statement Period").FontSize(9);
                    col.Item().Text($"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}").FontSize(9).SemiBold();
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Red.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, Vendor vendor, List<Bill> bills, DateTime startDate, DateTime endDate)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Vendor Information
            column.Item().Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("VENDOR INFORMATION").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Vendor ID: {vendor.Id}").FontSize(9);
                    col.Item().Text($"Name: {vendor.Name}").FontSize(9);
                    if (!string.IsNullOrEmpty(vendor.Email))
                    {
                        col.Item().Text($"Email: {vendor.Email}").FontSize(9);
                    }
                });
            });

            column.Item().PaddingTop(20);

            // Account Summary
            var totalBills = bills.Sum(b => b.TotalAmount);
            var totalBalance = totalBills; // Simplified - bills don't track payments same way

            column.Item().Background(Colors.Red.Lighten5).Padding(15).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("ACCOUNT SUMMARY").FontSize(11).Bold();
                    col.Item().PaddingTop(8).Row(r =>
                    {
                        r.RelativeItem().Text("Total Bills:").FontSize(9);
                        r.ConstantItem(100).AlignRight().Text($"${totalBills:N2}").FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(3).Row(r =>
                    {
                        r.RelativeItem().Text("Number of Bills:").FontSize(9);
                        r.ConstantItem(100).AlignRight().Text(bills.Count.ToString()).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(5).BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(5).Row(r =>
                    {
                        r.RelativeItem().Text("Outstanding:").FontSize(10).Bold();
                        r.ConstantItem(100).AlignRight().Text($"${totalBalance:N2}").FontSize(10).Bold()
                            .FontColor(Colors.Red.Darken2);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Transaction History
            if (bills.Any())
            {
                column.Item().Text("TRANSACTION HISTORY").FontSize(11).SemiBold();
                column.Item().PaddingTop(8).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(80);   // Date
                        columns.ConstantColumn(100);  // Bill #
                        columns.RelativeColumn(2);    // Description
                        columns.ConstantColumn(80);   // Amount
                        columns.ConstantColumn(80);   // Status
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Date").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Bill #").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Description").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Amount").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Status").FontSize(9).SemiBold();
                    });

                    // Rows
                    var isAlternate = false;

                    foreach (var bill in bills.OrderBy(b => b.BillDate))
                    {
                        var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                        table.Cell().Background(bgColor).Padding(4)
                            .Text(bill.BillDate.ToString("MM/dd/yyyy")).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(bill.BillNumber).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(bill.Description ?? "Vendor Bill").FontSize(8);
                        table.Cell().Background(bgColor).Padding(4).AlignRight()
                            .Text(bill.TotalAmount.ToString("N2")).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(bill.Status).FontSize(8)
                            .FontColor(bill.Status == "Paid" ? Colors.Green.Darken2 : Colors.Orange.Darken2);

                        isAlternate = !isAlternate;
                    }

                    // Total Row
                    table.Cell().ColumnSpan(3).Background(Colors.Grey.Lighten2).Padding(5)
                        .Text("STATEMENT TOTAL").FontSize(9).Bold().AlignRight();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                        .Text(totalBills.ToString("N2")).FontSize(9).Bold();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(5);
                });
            }
            else
            {
                column.Item().PaddingTop(20).AlignCenter().Text("No bills for this period.")
                    .FontSize(10).Italic().FontColor(Colors.Grey.Darken1);
            }

            // Remittance Information
            column.Item().PaddingTop(30).Background(Colors.Blue.Lighten5).Padding(15).Column(col =>
            {
                col.Item().Text("REMITTANCE INFORMATION").FontSize(10).Bold();
                col.Item().PaddingTop(5).Text("For payments, please remit to:").FontSize(9);
                col.Item().PaddingTop(5).Text("YOUR COMPANY NAME").FontSize(9).SemiBold();
                col.Item().Text("Accounts Payable Department").FontSize(9);
                col.Item().Text("123 Business Street, City, State 12345").FontSize(9);
            });
        });
    }

    private static void ComposeFooter(IContainer container)
    {
        container.BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10).Row(row =>
        {
            row.RelativeItem().Text("For questions about this statement, please contact accounts@company.com")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
            row.RelativeItem().AlignRight().Text($"Generated on {DateTime.Now:MMM dd, yyyy}")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
        });
    }
}

/// <summary>
/// Specification for filtering bills for vendor statements.
/// </summary>
internal sealed class BillsForStatementSpec : Specification<Bill>
{
    public BillsForStatementSpec(DefaultIdType vendorId, DateTime startDate, DateTime endDate)
    {
        Query.Where(b => b.VendorId == vendorId && b.BillDate >= startDate && b.BillDate <= endDate);
        Query.OrderBy(b => b.BillDate);
    }
}
