using Accounting.Application.Reports.DebitMemo.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating individual Debit Memo PDF reports using QuestPDF.
/// </summary>
public sealed class DebitMemoReportService : IDebitMemoReportService
{
    private readonly IReadRepository<DebitMemo> _debitMemoRepository;

    public DebitMemoReportService(
        [FromKeyedServices("accounting:debitmemos")] IReadRepository<DebitMemo> debitMemoRepository)
    {
        _debitMemoRepository = debitMemoRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType debitMemoId)
    {
        var debitMemo = await _debitMemoRepository.GetByIdAsync(debitMemoId);
        if (debitMemo == null)
        {
            throw new InvalidOperationException($"Debit Memo with ID {debitMemoId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, debitMemo));
                page.Content().Element(c => ComposeContent(c, debitMemo));
                page.Footer().Element(c => ComposeFooter(c, debitMemo));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, DebitMemo debitMemo)
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
                    col.Item().Text("DEBIT MEMO").FontSize(20).Bold().FontColor(Colors.Red.Darken2);
                    col.Item().PaddingTop(5).Text($"# {debitMemo.MemoNumber}").FontSize(11).SemiBold();
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Red.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, DebitMemo debitMemo)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Debit Memo Details Section
            column.Item().Row(row =>
            {
                // Vendor/Reference Info
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("DEBIT ISSUED TO:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Reference Type: {debitMemo.ReferenceType}").FontSize(9);
                    col.Item().Text($"Reference ID: {debitMemo.ReferenceId}").FontSize(9);
                    if (debitMemo.OriginalDocumentId.HasValue)
                    {
                        col.Item().Text($"Original Document: {debitMemo.OriginalDocumentId}").FontSize(9);
                    }
                });

                // Memo Info
                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Memo Date:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(debitMemo.MemoDate.ToString("MMM dd, yyyy")).FontSize(9).SemiBold();
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Status:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(debitMemo.Status).FontSize(9).SemiBold()
                            .FontColor(debitMemo.Status == "Applied" ? Colors.Green.Darken2 : Colors.Orange.Darken2);
                    });
                    col.Item().PaddingTop(2).Row(r =>
                    {
                        r.RelativeItem().Text("Approval:").FontSize(9);
                        r.RelativeItem().AlignRight().Text(debitMemo.ApprovalStatus).FontSize(9).SemiBold()
                            .FontColor(debitMemo.ApprovalStatus == "Approved" ? Colors.Green.Darken2 : Colors.Grey.Darken1);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Reason for Debit
            if (!string.IsNullOrEmpty(debitMemo.Reason))
            {
                column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(col =>
                {
                    col.Item().Text("REASON FOR DEBIT:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(debitMemo.Reason).FontSize(9);
                });
                
                column.Item().PaddingTop(15);
            }

            // Debit Amount Details
            column.Item().Row(row =>
            {
                row.RelativeItem(); // Spacer

                row.ConstantItem(300).Column(col =>
                {
                    // Debit Amount
                    col.Item().Background(Colors.Red.Lighten4).Padding(10).Row(r =>
                    {
                        r.RelativeItem().Text("DEBIT AMOUNT:").FontSize(12).Bold();
                        r.ConstantItem(120).AlignRight().Text(debitMemo.Amount.ToString("N2")).FontSize(12).Bold();
                    });

                    col.Item().PaddingTop(10);

                    // Applied Amount
                    if (debitMemo.AppliedAmount > 0)
                    {
                        col.Item().Row(r =>
                        {
                            r.RelativeItem().Text("Applied Amount:").FontSize(10);
                            r.ConstantItem(120).AlignRight().Text($"({debitMemo.AppliedAmount:N2})").FontSize(10)
                                .FontColor(Colors.Red.Darken2);
                        });
                    }

                    col.Item().PaddingTop(8).BorderTop(1).BorderColor(Colors.Grey.Medium);

                    // Unapplied Amount
                    col.Item().PaddingTop(8).Background(Colors.Orange.Lighten4).Padding(8).Row(r =>
                    {
                        r.RelativeItem().Text("UNAPPLIED AMOUNT:").FontSize(11).Bold();
                        r.ConstantItem(120).AlignRight().Text(debitMemo.UnappliedAmount.ToString("N2")).FontSize(11).Bold()
                            .FontColor(Colors.Orange.Darken2);
                    });
                });
            });

            column.Item().PaddingTop(20);

            // Application Details
            if (debitMemo.IsApplied && debitMemo.AppliedDate.HasValue)
            {
                column.Item().Background(Colors.Red.Lighten5).Padding(10).Column(col =>
                {
                    col.Item().Text("APPLICATION DETAILS:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Applied on {debitMemo.AppliedDate.Value:MMM dd, yyyy}")
                        .FontSize(9).FontColor(Colors.Red.Darken2);
                });
                
                column.Item().PaddingTop(15);
            }

            // Approval Details
            if (debitMemo.ApprovalStatus == "Approved" && !string.IsNullOrEmpty(debitMemo.ApprovedBy))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("APPROVAL INFORMATION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Approved by: {debitMemo.ApprovedBy}").FontSize(9);
                    if (debitMemo.ApprovedDate.HasValue)
                    {
                        col.Item().Text($"Approved on: {debitMemo.ApprovedDate.Value:MMM dd, yyyy}").FontSize(9);
                    }
                });
                
                column.Item().PaddingTop(15);
            }

            // Description
            if (!string.IsNullOrEmpty(debitMemo.Description))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("DESCRIPTION:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(debitMemo.Description).FontSize(9);
                });
                
                column.Item().PaddingTop(15);
            }

            // Notes
            if (!string.IsNullOrEmpty(debitMemo.Notes))
            {
                column.Item().Column(col =>
                {
                    col.Item().Text("NOTES:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(debitMemo.Notes).FontSize(9).Italic();
                });
            }
        });
    }

    private static void ComposeFooter(IContainer container, DebitMemo debitMemo)
    {
        container.Column(column =>
        {
            column.Item().BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10);

            column.Item().AlignCenter().Text("This debit memo increases the amount owed by the vendor.")
                .FontSize(9).Italic().FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(5).AlignCenter().Text("For questions, please contact accounts@company.com")
                .FontSize(8).FontColor(Colors.Grey.Darken1);

            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Text($"Debit Memo #{debitMemo.MemoNumber}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                row.RelativeItem().AlignRight().Text($"Generated on {DateTime.Now:MMM dd, yyyy}")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
            });
        });
    }
}
