namespace Accounting.Application.Reports.BankAccountSummary.v1.Services;
public interface IBankAccountSummaryReportService { Task<byte[]> GenerateReportAsync(); }
