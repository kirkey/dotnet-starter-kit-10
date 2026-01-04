using Accounting.Application.Reports.Payment.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating Payment Receipt PDF reports using QuestPDF.
/// </summary>
public sealed class PaymentReportService : IPaymentReportService
{
    private readonly IReadRepository<Payment> _paymentRepository;

    public PaymentReportService(
        [FromKeyedServices("accounting:payments")] IReadRepository<Payment> paymentRepository)
    {
        _paymentRepository = paymentRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType paymentId)
    {
        var payment = await _paymentRepository.GetByIdAsync(paymentId);
        if (payment == null)
        {
            throw new InvalidOperationException($"Payment with ID {paymentId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, payment));
                page.Content().Element(c => ComposeContent(c, payment));
                page.Footer().Element(c => ComposeFooter(c, payment));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, Payment payment)
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
                    col.Item().Text("PAYMENT RECEIPT").FontSize(18).Bold().FontColor(Colors.Green.Darken2);
                    col.Item().PaddingTop(5).Text($"# {payment.PaymentNumber}").FontSize(11).SemiBold();
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Green.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, Payment payment)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Payment Details
            column.Item().Row(row =>
            {
                // Payment Info
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("PAYMENT DETAILS:").FontSize(10).SemiBold();
                    if (payment.MemberId.HasValue)
                    {
                        col.Item().PaddingTop(5).Text($"Member ID: {payment.MemberId}").FontSize(9);
                    }
                    if (!string.IsNullOrEmpty(payment.ReferenceNumber))
                    {
                        col.Item().Text($"Reference: {payment.ReferenceNumber}").FontSize(9);
                    }
                });

                // Payment Info
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Payment Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(payment.PaymentDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Payment Method:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(payment.PaymentMethod).FontSize(9).SemiBold();
                    });
                });
            });

            column.Item().PaddingTop(25);

            // Payment Amount - Prominent Display
            column.Item().AlignCenter().Background(Colors.Green.Lighten5).Padding(20).Column(col =>
            {
                col.Item().AlignCenter().Text("AMOUNT PAID").FontSize(12).SemiBold().FontColor(Colors.Green.Darken2);
                col.Item().AlignCenter().PaddingTop(10).Text($"${payment.Amount:N2}")
                    .FontSize(32).Bold().FontColor(Colors.Green.Darken3);
            });

            column.Item().PaddingTop(25);

            // Allocation Details
            if (payment.Allocations.Any())
            {
                column.Item().Text("PAYMENT ALLOCATION").FontSize(11).SemiBold();
                column.Item().PaddingTop(8).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(3);
                        columns.ConstantColumn(120);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Type").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5)
                            .Text("Reference").FontSize(9).SemiBold();
                        header.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                            .Text("Amount").FontSize(9).SemiBold();
                    });

                    var isAlternate = false;
                    foreach (var allocation in payment.Allocations)
                    {
                        var bgColor = isAlternate ? Colors.Grey.Lighten4 : Colors.White;

                        table.Cell().Background(bgColor).Padding(5)
                            .Text($"Invoice: {allocation.InvoiceId}").FontSize(9);
                        table.Cell().Background(bgColor).Padding(5)
                            .Text($"Allocation ID: {allocation.Id}").FontSize(9);
                        table.Cell().Background(bgColor).Padding(5).AlignRight()
                            .Text(allocation.Amount.ToString("N2")).FontSize(9);

                        isAlternate = !isAlternate;
                    }

                    // Total row
                    table.Cell().ColumnSpan(2).Background(Colors.Grey.Lighten2).Padding(5)
                        .Text("TOTAL ALLOCATED").FontSize(9).SemiBold().AlignRight();
                    table.Cell().Background(Colors.Grey.Lighten2).Padding(5).AlignRight()
                        .Text(payment.Allocations.Sum(a => a.Amount).ToString("N2")).FontSize(9).SemiBold();
                });
            }

            column.Item().PaddingTop(20);

            // Description
            if (!string.IsNullOrEmpty(payment.Description))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("DESCRIPTION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(payment.Description).FontSize(9);
                });
                
                column.Item().PaddingTop(15);
            }

            // Notes
            if (!string.IsNullOrEmpty(payment.Notes))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("NOTES:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(payment.Notes).FontSize(9).Italic();
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, Payment payment)
    {
        container.Column(column =>
        {
            column.Item().BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10);

            column.Item().AlignCenter().Text("Thank you for your payment!")
                .FontSize(10).SemiBold().FontColor(Colors.Green.Darken2);

            column.Item().PaddingTop(5).AlignCenter().Text("Please retain this receipt for your records.")
                .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Text($"Receipt #{payment.PaymentNumber}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                row.RelativeItem().AlignRight().Text($"Printed on {DateTime.Now:MMM dd, yyyy}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }
}
