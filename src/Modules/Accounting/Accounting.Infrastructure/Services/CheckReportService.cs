using Accounting.Application.Reports.Check.v1.Services;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Service implementation for generating printable Check PDF reports using QuestPDF.
/// </summary>
public sealed class CheckReportService : ICheckReportService
{
    private readonly IReadRepository<Check> _checkRepository;

    public CheckReportService(
        [FromKeyedServices("accounting:checks")] IReadRepository<Check> checkRepository)
    {
        _checkRepository = checkRepository;
        QuestPDF.Settings.License = LicenseType.Community;
    }

    /// <inheritdoc/>
    public async Task<byte[]> GenerateReportAsync(DefaultIdType checkId)
    {
        var check = await _checkRepository.GetByIdAsync(checkId);
        if (check == null)
        {
            throw new InvalidOperationException($"Check with ID {checkId} not found.");
        }

        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.Letter); // Standard US check size
                page.Margin(0);
                page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                page.Content().Element(c => ComposeCheck(c, check));
            });
        });

        return document.GeneratePdf();
    }

    private static void ComposeCheck(IContainer container, Check check)
    {
        container.Padding(40).Column(column =>
        {
            // Company Header
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("YOUR COMPANY NAME").FontSize(14).Bold();
                    col.Item().Text("123 Business Street").FontSize(9);
                    col.Item().Text("City, State 12345").FontSize(9);
                    col.Item().Text("Phone: (555) 123-4567").FontSize(9);
                });

                row.ConstantItem(150).AlignRight().Column(col =>
                {
                    col.Item().Text($"Check #{check.CheckNumber}").FontSize(11).Bold();
                    col.Item().PaddingTop(3).Text(check.IssuedDate?.ToString("MM/dd/yyyy") ?? "Not Issued").FontSize(10);
                });
            });

            column.Item().PaddingTop(30);

            // Pay to the Order of
            column.Item().Row(row =>
            {
                row.ConstantItem(120).Text("PAY TO THE ORDER OF:").FontSize(10).SemiBold();
                row.RelativeItem().BorderBottom(1).Padding(5)
                    .Text($"Vendor ID: {check.VendorId}").FontSize(11);
            });

            column.Item().PaddingTop(15);

            // Amount in Words and Figures
            column.Item().Row(row =>
            {
                row.RelativeItem().BorderBottom(1).Padding(5).Column(col =>
                {
                    col.Item().Text(ConvertAmountToWords(check.Amount ?? 0m)).FontSize(11).SemiBold();
                    col.Item().PaddingTop(2).Text("DOLLARS").FontSize(9);
                });

                row.ConstantItem(150).AlignRight().Padding(10).Column(col =>
                {
                    col.Item().Border(2).BorderColor(Colors.Black).Padding(8).AlignRight()
                        .Text($"${check.Amount ?? 0m:N2}").FontSize(14).Bold();
                });
            });

            column.Item().PaddingTop(30);

            // Bank Info
            column.Item().Row(row =>
            {
                row.RelativeItem().Column(col =>
                {
                    col.Item().Text("BANK NAME").FontSize(10).Bold();
                    col.Item().Text("Bank Address").FontSize(8);
                    col.Item().Text("City, State ZIP").FontSize(8);
                });

                row.ConstantItem(200).AlignRight().Column(col =>
                {
                    col.Item().BorderBottom(1).PaddingBottom(5).Text("Authorized Signature");
                    col.Item().PaddingTop(15).Text("__________________________").FontSize(9);
                });
            });

            column.Item().PaddingTop(30);

            // MICR Line (Magnetic Ink Character Recognition)
            column.Item().AlignCenter().Text($"⑆{check.CheckNumber}⑆  ⑈1234567890⑈  ⑉0000001234⑉")
                .FontSize(12).FontFamily("MICR");

            column.Item().PaddingTop(30).BorderTop(1).BorderColor(Colors.Grey.Lighten1);

            // Check Stub / Record
            column.Item().PaddingTop(15).Background(Colors.Grey.Lighten4).Padding(15).Column(stub =>
            {
                stub.Item().Text("CHECK STUB - RETAIN FOR YOUR RECORDS").FontSize(9).Bold();
                stub.Item().PaddingTop(10).Row(r =>
                {
                    r.RelativeItem().Column(c =>
                    {
                        c.Item().Text($"Check Number: {check.CheckNumber}").FontSize(9);
                        c.Item().Text($"Date: {check.IssuedDate?.ToString("MM/dd/yyyy") ?? "Not Issued"}").FontSize(9);
                        c.Item().Text($"Amount: ${check.Amount ?? 0m:N2}").FontSize(9);
                    });
                    r.RelativeItem().Column(c =>
                    {
                        c.Item().Text($"Vendor ID: {check.VendorId}").FontSize(9);
                        c.Item().Text($"Status: {check.Status}").FontSize(9);
                        if (!string.IsNullOrEmpty(check.Memo))
                        {
                            c.Item().Text($"Memo: {check.Memo}").FontSize(9);
                        }
                    });
                });
            });
        });
    }

    private static string ConvertAmountToWords(decimal amount)
    {
        if (amount == 0) return "ZERO";

        var dollars = (int)amount;
        var cents = (int)((amount - dollars) * 100);

        var words = ConvertNumberToWords(dollars);
        if (cents > 0)
        {
            words += $" AND {cents:00}/100";
        }
        else
        {
            words += " AND 00/100";
        }

        return words.ToUpper();
    }

    private static string ConvertNumberToWords(int number)
    {
        if (number == 0) return "ZERO";
        if (number < 0) return "MINUS " + ConvertNumberToWords(Math.Abs(number));

        string[] ones = { "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" };
        string[] teens = { "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
        string[] tens = { "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

        string words = "";

        if (number / 1000000 > 0)
        {
            words += ConvertNumberToWords(number / 1000000) + " MILLION ";
            number %= 1000000;
        }

        if (number / 1000 > 0)
        {
            words += ConvertNumberToWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }

        if (number / 100 > 0)
        {
            words += ConvertNumberToWords(number / 100) + " HUNDRED ";
            number %= 100;
        }

        if (number > 0)
        {
            if (number < 10)
                words += ones[number];
            else if (number < 20)
                words += teens[number - 10];
            else
            {
                words += tens[number / 10];
                if (number % 10 > 0)
                    words += "-" + ones[number % 10];
            }
        }

        return words.Trim();
    }
}
