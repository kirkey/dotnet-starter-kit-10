using Accounting.Application.Reports.Invoice.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating individual Invoice PDF reports using QuestPDF.
/// </summary>
public sealed class InvoiceReportService : IInvoiceReportService
{
    private readonly ISender _mediator;
    private readonly IReadRepository<Invoice> _invoiceRepository;

    public InvoiceReportService(
        ISender mediator,
        [FromKeyedServices("accounting:invoices")] IReadRepository<Invoice> invoiceRepository)
    {
        _mediator = mediator;
        _invoiceRepository = invoiceRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType invoiceId)
    {
        // Get invoice with line items
        var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
        if (invoice == null)
        {
            throw new InvalidOperationException($"Invoice with ID {invoiceId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, invoice));
                page.Content().Element(c => ComposeContent(c, invoice));
                page.Footer().Element(c => ComposeFooter(c, invoice));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, Invoice invoice)
    {
        container.Column(column =>
        {
            // Company Header
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("YOUR COMPANY NAME").FontSize(18).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(3).Text("123 Business Street").FontSize(9);
                    col.Item().Text("City, State 12345").FontSize(9);
                    col.Item().Text("Phone: (555) 123-4567").FontSize(9);
                    col.Item().Text("Email: billing@company.com").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().Text("INVOICE").FontSize(20).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(5).Text($"# {invoice.InvoiceNumber}").FontSize(11).SemiBold();
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, Invoice invoice)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Invoice Details Section
            column.Item().Row(row =>
            {
                // Bill To
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("BILL TO:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Member ID: {invoice.MemberId}").FontSize(9);
                    col.Item().Text($"Billing Period: {invoice.BillingPeriod}").FontSize(9);
                });

                // Invoice Info
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Invoice Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(invoice.InvoiceDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Due Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(invoice.DueDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Status:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(invoice.Status).FontSize(9).SemiBold()
                            .FontColor(invoice.Status == "Paid" ? Colors.Green.Darken2 : Colors.Orange.Darken2);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Line Items Table
            if (invoice.LineItems.Any())
            {
                column.Item().Text("LINE ITEMS").FontSize(11).SemiBold();
                column.Item().PaddingTop(8).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);      // Description
                        columns.ConstantColumn(80);     // Quantity
                        columns.ConstantColumn(80);     // Unit Price
                        columns.ConstantColumn(100);    // Total
                    });

                    // Header
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

                    // Rows
                    var isAlternate = false;
                    foreach (var lineItem in invoice.LineItems)
                    {
                        var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                        table.Cell().Background(bgColor).Padding(5)
                            .Text(lineItem.Description ?? "N/A").FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text(lineItem.Quantity.ToString("N2")).FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text(lineItem.UnitPrice.ToString("N2")).FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text(lineItem.TotalPrice.ToString("N2")).FontSize(9);

                        isAlternate = !isAlternate;
                    }
                });

                column.Item().PaddingTop(15);
            }

            // Charges Summary
            column.Item().Row(row =>
            {
                row.RelativeItem(); // Spacer

                row.ConstantItem(300).Column(col =>
                {
                    if (invoice.UsageCharge > 0)
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Usage Charge:").FontSize(9);
                            r.ConstantItem(100).AlignRight().Text(invoice.UsageCharge.ToString("N2")).FontSize(9);
                        });
                        col.Item().Text($"({invoice.KWhUsed:N2} kWh)").FontSize(8).Italic().AlignRight();
                    }

                    if (invoice.BasicServiceCharge > 0)
                    {
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.RelativeItem().Text("Basic Service Charge:").FontSize(9);
                            r.ConstantItem(100).AlignRight().Text(invoice.BasicServiceCharge.ToString("N2")).FontSize(9);
                        });
                    }

                    if (invoice.DemandCharge.HasValue && invoice.DemandCharge.Value > 0)
                    {
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.RelativeItem().Text("Demand Charge:").FontSize(9);
                            r.ConstantItem(100).AlignRight().Text(invoice.DemandCharge.Value.ToString("N2")).FontSize(9);
                        });
                    }

                    if (invoice.OtherCharges > 0)
                    {
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.RelativeItem().Text("Other Charges:").FontSize(9);
                            r.ConstantItem(100).AlignRight().Text(invoice.OtherCharges.ToString("N2")).FontSize(9);
                        });
                    }

                    if (invoice.LateFee.HasValue && invoice.LateFee.Value > 0)
                    {
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.RelativeItem().Text("Late Fee:").FontSize(9).FontColor(Colors.Red.Darken1);
                            r.ConstantItem(100).AlignRight().Text(invoice.LateFee.Value.ToString("N2")).FontSize(9).FontColor(Colors.Red.Darken1);
                        });
                    }

                    if (invoice.TaxAmount > 0)
                    {
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.RelativeItem().Text("Tax:").FontSize(9);
                            r.ConstantItem(100).AlignRight().Text(invoice.TaxAmount.ToString("N2")).FontSize(9);
                        });
                    }

                    col.Item().PaddingTop(8).BorderTop(1).BorderColor(Colors.Grey.Medium);

                    // Total Amount
                    col.Item().PaddingTop(5).Background(Colors.Blue.Lighten4).Padding(8).Row(r =>
                    {
                        r.RelativeItem().Text("TOTAL AMOUNT:").FontSize(11).Bold();
                        r.ConstantItem(100).AlignRight().Text(invoice.TotalAmount.ToString("N2")).FontSize(11).Bold();
                    });

                    // Paid Amount
                    if (invoice.PaidAmount > 0)
                    {
                        col.Item().PaddingTop(5).Row(r =>
                        {
                            r.RelativeItem().Text("Paid Amount:").FontSize(10).FontColor(Colors.Green.Darken2);
                            r.ConstantItem(100).AlignRight().Text(invoice.PaidAmount.ToString("N2")).FontSize(10).FontColor(Colors.Green.Darken2);
                        });
                    }

                    // Outstanding Amount
                    var outstanding = invoice.GetOutstandingAmount();
                    if (outstanding > 0)
                    {
                        col.Item().PaddingTop(5).Background(Colors.Orange.Lighten4).Padding(8).Row(r =>
                        {
                            r.RelativeItem().Text("AMOUNT DUE:").FontSize(11).Bold().FontColor(Colors.Orange.Darken2);
                            r.ConstantItem(100).AlignRight().Text(outstanding.ToString("N2")).FontSize(11).Bold().FontColor(Colors.Orange.Darken2);
                        });
                    }
                });
            });

            // Notes
            if (!string.IsNullOrEmpty(invoice.Notes))
            {
                column.Item().PaddingTop(20).Column(col =>
                {
                    col.Item().Text("NOTES:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(invoice.Notes).FontSize(9);
                });
            }

            // Payment Info
            if (!string.IsNullOrEmpty(invoice.PaymentMethod) && invoice.PaidDate.HasValue)
            {
                column.Item().PaddingTop(15).Column(col =>
                {
                    col.Item().Text("PAYMENT INFORMATION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Paid on {invoice.PaidDate.Value:MMM dd, yyyy} via {invoice.PaymentMethod}")
                        .FontSize(9).FontColor(Colors.Green.Darken2);
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, Invoice invoice)
    {
        container.Column(column =>
        {
            column.Item().BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10);

            column.Item().AlignCenter().Text("Thank you for your business!")
                .FontSize(10).Italic().FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(5).AlignCenter().Text("For questions about this invoice, please contact billing@company.com")
                .FontSize(8).FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Text($"Invoice #{invoice.InvoiceNumber}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                row.RelativeItem().AlignRight().Text($"Generated on {DateTime.Now:MMM dd, yyyy}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }
}
