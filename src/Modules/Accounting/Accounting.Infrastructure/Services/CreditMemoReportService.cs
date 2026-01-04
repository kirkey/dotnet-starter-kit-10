using Accounting.Application.Reports.CreditMemo.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating individual Credit Memo PDF reports using QuestPDF.
/// </summary>
public sealed class CreditMemoReportService : ICreditMemoReportService
{
    private readonly IReadRepository<CreditMemo> _creditMemoRepository;

    public CreditMemoReportService(
        [FromKeyedServices("accounting:creditmemos")] IReadRepository<CreditMemo> creditMemoRepository)
    {
        _creditMemoRepository = creditMemoRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType creditMemoId)
    {
        var creditMemo = await _creditMemoRepository.GetByIdAsync(creditMemoId);
        if (creditMemo == null)
        {
            throw new InvalidOperationException($"Credit Memo with ID {creditMemoId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, creditMemo));
                page.Content().Element(c => ComposeContent(c, creditMemo));
                page.Footer().Element(c => ComposeFooter(c, creditMemo));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, CreditMemo creditMemo)
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
                    col.Item().Text("Email: billing@company.com").FontSize(9);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().Text("CREDIT MEMO").FontSize(20).Bold().FontColor(Colors.Green.Darken2);
                    col.Item().PaddingTop(5).Text($"# {creditMemo.MemoNumber}").FontSize(11).SemiBold();
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Green.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, CreditMemo creditMemo)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Credit Memo Details Section
            column.Item().Row(row =>
            {
                // Customer/Reference Info
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("CREDIT ISSUED TO:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Reference Type: {creditMemo.ReferenceType}").FontSize(9);
                    col.Item().Text($"Reference ID: {creditMemo.ReferenceId}").FontSize(9);
                    if (creditMemo.OriginalDocumentId.HasValue)
                    {
                        col.Item().Text($"Original Document: {creditMemo.OriginalDocumentId}").FontSize(9);
                    }
                });

                // Memo Info
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Memo Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(creditMemo.MemoDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Status:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(creditMemo.Status).FontSize(9).SemiBold()
                            .FontColor(creditMemo.Status == "Applied" ? Colors.Green.Darken2 : Colors.Orange.Darken2);
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Approval:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(creditMemo.ApprovalStatus).FontSize(9).SemiBold()
                            .FontColor(creditMemo.ApprovalStatus == "Approved" ? Colors.Green.Darken2 : Colors.Grey.Darken1);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Reason for Credit
            if (!string.IsNullOrEmpty(creditMemo.Reason))
            {
                column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(col =>
                {
                    col.Item().Text("REASON FOR CREDIT:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(creditMemo.Reason).FontSize(9);
                });
                
                column.Item().PaddingTop(15);
            }

            // Credit Amount Details
            column.Item().Row(row =>
            {
                row.RelativeItem(); // Spacer

                row.ConstantItem(300).Column(col =>
                {
                    // Credit Amount
                    col.Item().Background(Colors.Green.Lighten4).Padding(10).Row(r =>
                    {
                        r.RelativeItem().Text("CREDIT AMOUNT:").FontSize(12).Bold();
                        r.ConstantItem(120).AlignRight().Text(creditMemo.Amount.ToString("N2")).FontSize(12).Bold();
                    });

                    col.Item().PaddingTop(10);

                    // Applied Amount
                    if (creditMemo.AppliedAmount > 0)
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Applied Amount:").FontSize(10);
                            r.ConstantItem(120).AlignRight().Text($"({creditMemo.AppliedAmount:N2})").FontSize(10)
                                .FontColor(Colors.Green.Darken2);
                        });
                    }

                    // Refunded Amount
                    if (creditMemo.RefundedAmount > 0)
                    {
                        col.Item().PaddingTop(3).Row(r =>
                        {
                            r.RelativeItem().Text("Refunded Amount:").FontSize(10);
                            r.ConstantItem(120).AlignRight().Text($"({creditMemo.RefundedAmount:N2})").FontSize(10)
                                .FontColor(Colors.Blue.Darken2);
                        });
                    }

                    col.Item().PaddingTop(8).BorderTop(1).BorderColor(Colors.Grey.Medium);

                    // Unapplied Amount
                    col.Item().PaddingTop(8).Background(Colors.Orange.Lighten4).Padding(8).Row(r =>
                    {
                        r.RelativeItem().Text("AVAILABLE CREDIT:").FontSize(11).Bold();
                        r.ConstantItem(120).AlignRight().Text(creditMemo.UnappliedAmount.ToString("N2")).FontSize(11).Bold()
                            .FontColor(Colors.Orange.Darken2);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Application Details
            if (creditMemo.IsApplied && creditMemo.AppliedDate.HasValue)
            {
                column.Item().Background(Colors.Green.Lighten5).Padding(10).Column(col =>
                {
                    col.Item().Text("APPLICATION DETAILS:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Applied on {creditMemo.AppliedDate.Value:MMM dd, yyyy}")
                        .FontSize(9).FontColor(Colors.Green.Darken2);
                });
                
                column.Item().PaddingTop(15);
            }

            // Approval Details
            if (creditMemo.ApprovalStatus == "Approved" && !string.IsNullOrEmpty(creditMemo.ApprovedBy))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("APPROVAL INFORMATION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Approved by: {creditMemo.ApprovedBy}").FontSize(9);
                    if (creditMemo.ApprovedDate.HasValue)
                    {
                        col.Item().Text($"Approved on: {creditMemo.ApprovedDate.Value:MMM dd, yyyy}").FontSize(9);
                    }
                });
                
                column.Item().PaddingTop(15);
            }

            // Description
            if (!string.IsNullOrEmpty(creditMemo.Description))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("DESCRIPTION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(creditMemo.Description).FontSize(9);
                });
                
                column.Item().PaddingTop(15);
            }

            // Notes
            if (!string.IsNullOrEmpty(creditMemo.Notes))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("NOTES:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(creditMemo.Notes).FontSize(9).Italic();
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, CreditMemo creditMemo)
    {
        container.Column(column =>
        {
            column.Item().BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10);

            column.Item().AlignCenter().Text("This credit memo can be applied to future purchases or refunded per company policy.")
                .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(5).AlignCenter().Text("For questions, please contact billing@company.com")
                .FontSize(8).FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Text($"Credit Memo #{creditMemo.MemoNumber}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                row.RelativeItem().AlignRight().Text($"Generated on {DateTime.Now:MMM dd, yyyy}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }
}
