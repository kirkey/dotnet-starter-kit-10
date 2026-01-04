using Accounting.Application.Reports.BankReconciliation.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

public sealed class BankReconciliationReportService : IBankReconciliationReportService
{
    private readonly IReadRepository<AccountReconciliation> _reconciliationRepository;

    public BankReconciliationReportService(
        [FromKeyedServices("accounting:account-reconciliations")] IReadRepository<AccountReconciliation> reconciliationRepository)
    {
        _reconciliationRepository = reconciliationRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<byte[]> GenerateReportAsync(DefaultIdType reconciliationId)
    {
        var reconciliation = await _reconciliationRepository.GetByIdAsync(reconciliationId);
        if (reconciliation == null)
        {
            throw new InvalidOperationException($"Bank Reconciliation with ID {reconciliationId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(40);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Header().Element(c => ComposeHeader(c, reconciliation));
                page.Content().Element(c => ComposeContent(c, reconciliation));
                page.Footer().Element(ComposeFooter);
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeHeader(IContainer container, AccountReconciliation reconciliation)
    {
        container.Column(column =>
        {
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("ACCOUNT RECONCILIATION").FontSize(18).Bold().FontColor(Colors.Blue.Darken2);
                    col.Item().PaddingTop(5).Text($"GL Account: {reconciliation.GeneralLedgerAccountId}").FontSize(10);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().Text($"Date: {reconciliation.ReconciliationDate:MMM dd, yyyy}").FontSize(10).SemiBold();
                    col.Item().Text($"Status: {reconciliation.ReconciliationStatus}").FontSize(9)
                        .FontColor(reconciliation.ReconciliationStatus == "Reconciled" ? Colors.Green.Darken2 : Colors.Orange.Darken2);
                });
            });

            column.Item().PaddingTop(15).BorderBottom(2).BorderColor(Colors.Blue.Darken2);
        });
    }

    private static void ComposeContent(IContainer container, AccountReconciliation reconciliation)
    {
        container.PaddingTop(20).Column(column =>
        {
            // Summary Section
            column.Item().Background(Colors.Blue.Lighten5).Padding(15).Column(summary =>
            {
                summary.Item().Text("RECONCILIATION SUMMARY").FontSize(11).Bold();
                summary.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Text("GL Balance:").FontSize(9);
                    r.ConstantItem(120).AlignRight().Text($"${reconciliation.GlBalance:N2}").FontSize(9).SemiBold();
                });
                summary.Item().PaddingTop(3).Row(r =>
                {
                    r.RelativeItem().Text("Subsidiary Ledger Balance:").FontSize(9);
                    r.ConstantItem(120).AlignRight().Text($"${reconciliation.SubsidiaryLedgerBalance:N2}").FontSize(9).SemiBold();
                });
                summary.Item().PaddingTop(3).Row(r =>
                {
                    r.RelativeItem().Text("Variance:").FontSize(9);
                    r.ConstantItem(120).AlignRight().Text($"${reconciliation.Variance:N2}").FontSize(9).SemiBold()
                        .FontColor(Math.Abs(reconciliation.Variance) < 0.01m ? Colors.Green.Darken2 : Colors.Red.Darken2);
                });
            });

            column.Item().PaddingTop(20);

            // Outstanding Items
            column.Item().Text("RECONCILIATION DETAILS").FontSize(11).SemiBold();
            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Outstanding Deposits").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Count: 0").FontSize(9);
                    col.Item().Text($"Amount: $0.00").FontSize(9);
                });
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("Outstanding Checks").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text($"Count: 0").FontSize(9);
                    col.Item().Text($"Amount: $0.00").FontSize(9);
                });
            });

            column.Item().PaddingTop(20);

            // Notes
            if (!string.IsNullOrEmpty(reconciliation.Notes))
            {
                column.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(col =>
                {
                    col.Item().Text("NOTES:").FontSize(10).SemiBold();
                    col.Item().PaddingTop(5).Text(reconciliation.Notes).FontSize(9);
                });
            }

            // Reconciliation Status
            column.Item().PaddingTop(20).AlignCenter().Text(
                reconciliation.ReconciliationStatus == "Reconciled" 
                    ? "✓ This reconciliation is complete and balanced." 
                    : "⚠ This reconciliation is pending completion.")
                .FontSize(10).SemiBold()
                .FontColor(reconciliation.ReconciliationStatus == "Reconciled" ? Colors.Green.Darken2 : Colors.Orange.Darken2);
        });
    }

    private static void ComposeFooter(IContainer container)
    {
        container.BorderTop(1).BorderColor(Colors.Grey.Medium).PaddingTop(10).Row(row =>
        {
            row.RelativeItem().Text("Bank Reconciliation Report")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
            row.RelativeItem().AlignRight().Text($"Generated: {DateTime.Now:MMM dd, yyyy}")
                .FontSize(8).FontColor(Colors.Grey.Darken1);
        });
    }
}
