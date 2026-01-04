using Accounting.Application.Reports.CustomerStatement.v1.Services;
using Ardalis.Specification;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Customer Statement PDF reports using QuestPDF.
/// </summary>
public sealed class CustomerStatementReportService : ICustomerStatementReportService
{
    private readonly IReadRepository<Customer> _customerRepository;
    private readonly IReadRepository<Invoice> _invoiceRepository;

    public CustomerStatementReportService(
        [FromKeyedServices("accounting:customers")] IReadRepository<Customer> customerRepository,
        [FromKeyedServices("accounting:invoices")] IReadRepository<Invoice> invoiceRepository)
    {
        _customerRepository = customerRepository;
        _invoiceRepository = invoiceRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType customerId, DateTime startDate, DateTime endDate)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {customerId} not found.");
        }

        // Get invoices for the period using specification
        var spec = new InvoicesForStatementSpec(customerId, startDate, endDate);
        var invoices = await _invoiceRepository.ListAsync(spec);

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, customer, startDate, endDate));
                page.Content().Element(c => ComposeContent(c, customer, invoices.ToList(), startDate, endDate));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, Customer customer, DateTime startDate, DateTime endDate)
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
                    col.Item().Text("CUSTOMER STATEMENT").FontSize(16).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(5).Text($"Statement Period").FontSize(9);
                    col.Item().Text($"{startDate:MMM dd, yyyy} - {endDate:MMM dd, yyyy}").FontSize(9).SemiBold();
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, Customer customer, List<Invoice> invoices, DateTime startDate, DateTime endDate)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Customer Information
            column.Item().Background(Colors.Grey.Lighten4).Padding(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("CUSTOMER INFORMATION").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Customer ID: {customer.Id}").FontSize(9);
                    col.Item().Text($"Name: {customer.Name}").FontSize(9);
                    if (!string.IsNullOrEmpty(customer.Email))
                    {
                        col.Item().Text($"Email: {customer.Email}").FontSize(9);
                    }
                });
            });

            column.Item().PaddingTop(20);

            // Account Summary
            var totalCharges = invoices.Sum(i => i.TotalAmount);
            var totalPayments = invoices.Sum(i => i.PaidAmount);
            var totalBalance = totalCharges - totalPayments;

            column.Item().Background(Colors.Blue.Lighten5).Padding(15).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("ACCOUNT SUMMARY").FontSize(11).Bold();
                    col.Item().PaddingTop(8).Row(r =>
                    {
                        r.RelativeItem().Text("Total Charges:").FontSize(9);
                        r.ConstantItem(100).AlignRight().Text($"${totalCharges:N2}").FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(3).Row(r =>
                    {
                        r.RelativeItem().Text("Total Payments:").FontSize(9);
                        r.ConstantItem(100).AlignRight().Text($"${totalPayments:N2}").FontSize(9).SemiBold().FontColor(Colors.Green.Darken2);
                    });
                    col.Item().PaddingTop(5).BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(5).Row(r =>
                    {
                        r.RelativeItem().Text("Balance Due:").FontSize(10).Bold();
                        r.ConstantItem(100).AlignRight().Text($"${totalBalance:N2}").FontSize(10).Bold()
                            .FontColor(totalBalance > 0 ? Colors.Red.Darken2 : Colors.Green.Darken2);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Transaction History
            if (invoices.Any())
            {
                column.Item().Text("TRANSACTION HISTORY").FontSize(11).SemiBold();
                column.Item().PaddingTop(8).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(80);   // Date
                        columns.ConstantColumn(100);  // Invoice #
                        columns.RelativeColumn(2);    // Description
                        columns.ConstantColumn(80);   // Charges
                        columns.ConstantColumn(80);   // Payments
                        columns.ConstantColumn(80);   // Balance
                    });

                    // Header
                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Date").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Invoice #").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Description").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Charges").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Payments").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Balance").FontSize(9).SemiBold();
                    });

                    // Rows
                    var isAlternate = false;
                    decimal runningBalance = 0;

                    foreach (var invoice in invoices.OrderBy(i => i.InvoiceDate))
                    {
                        var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;
                        runningBalance += invoice.TotalAmount - invoice.PaidAmount;

                        table.Cell().Background(bgColor).Padding(4)
                            .Text(invoice.InvoiceDate.ToString("MM/dd/yyyy")).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(invoice.InvoiceNumber).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4)
                            .Text(invoice.Description ?? "Invoice").FontSize(8);
                        table.Cell().Background(bgColor).Padding(4).AlignRight()
                            .Text(invoice.TotalAmount.ToString("N2")).FontSize(8);
                        table.Cell().Background(bgColor).Padding(4).AlignRight()
                            .Text(invoice.PaidAmount > 0 ? invoice.PaidAmount.ToString("N2") : "-").FontSize(8);
                        table.Cell().Background(bgColor).Padding(4).AlignRight()
                            .Text(runningBalance.ToString("N2")).FontSize(8)
                            .FontColor(runningBalance > 0 ? Colors.Red.Darken1 : Colors.Green.Darken2);

                        isAlternate = !isAlternate;
                    }

                    // Total Row
                    table.Cell().ColumnSpan(3).Background(Colors.Grey.Lighten2).Padding(5)
                        .Text("STATEMENT TOTALS").FontSize(9).Bold().AlignRight();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                        .Text(totalCharges.ToString("N2")).FontSize(9).Bold();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                        .Text(totalPayments.ToString("N2")).FontSize(9).Bold();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                        .Text(totalBalance.ToString("N2")).FontSize(9).Bold();
                });
            }
            else
            {
                column.Item().PaddingTop(20).AlignCenter().Text("No transactions for this period.")
                    .FontSize(10).Italic().FontColor(Colors.Grey.Darken1);
            }

            // Payment Instructions
            if (totalBalance > 0)
            {
                column.Item().PaddingTop(30).Background(Colors.Orange.Lighten5).Padding(15).Column(col =>
                {
                    col.Item().Text("PAYMENT INFORMATION").FontSize(10).Bold();
                    col.Item().PaddingTop(5).Text($"Amount Due: ${totalBalance:N2}").FontSize(11).SemiBold();
                    col.Item().PaddingTop(5).Text("Please remit payment to the address above.").FontSize(9);
                    col.Item().Text("Make checks payable to: YOUR COMPANY NAME").FontSize(9);
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container)
    {
        container.BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10).Row(row =>
        {
            row.RelativeItem().Text("For questions about this statement, please contact billing@company.com")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
            row.RelativeItem().AlignRight().Text($"Generated on {DateTime.Now:MMM dd, yyyy}")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
        });
    }
}

/// <summary>
/// Specification for filtering invoices for customer statements.
/// </summary>
internal sealed class InvoicesForStatementSpec : Specification<Invoice>
{
    public InvoicesForStatementSpec(DefaultIdType customerId, DateTime startDate, DateTime endDate)
    {
        Query.Where(i => i.MemberId == customerId && i.InvoiceDate >= startDate && i.InvoiceDate <= endDate);
        Query.OrderBy(i => i.InvoiceDate);
    }
}
